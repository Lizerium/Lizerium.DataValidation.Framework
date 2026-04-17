/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 17 апреля 2026 06:51:46
 * Version: 1.0.7
 */

using System.Diagnostics;
using System.Linq;
using System.Text;

using Lizerium.Tests.Components;

using LizeriumTests.Logic;
using LizeriumTests.Logic.Collections;
using LizeriumTests.Logic.CRC;
using LizeriumTests.Logic.XML;

using Newtonsoft.Json;

namespace Lizerium.Tests.GLOBAL
{
    [TestClass]
    public sealed class GlobalTests
    {
        private static StreamWriter? _logWriter;
        private static string? _logFileDir;
        private static string? _logFilePath;
        private bool isInputDataExists { get; set; }
        private static SettingsService Settings { get; set; }
        private static bool _settingsValid = true;

        /// <summary>
        /// Выполняется один раз перед всеми методами в этом классе
        /// </summary>
        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {
            var outputDir = AppDomain.CurrentDomain.BaseDirectory;
            var dirTests = Path.Combine(outputDir, "TESTS_LOGGING");
            if (!Directory.Exists(dirTests)) Directory.CreateDirectory(dirTests);
            _logFileDir = dirTests;
            _logFilePath = Path.Combine(dirTests, "GlobalTests.ini");
            File.WriteAllText(_logFilePath, "");
            _logWriter = new StreamWriter(_logFilePath, append: true) { AutoFlush = true };
            Extensions.Log("🉐 Единоразовая подготовка логирования завершена...", _logWriter);
            Settings = new SettingsService();
            if (!Settings.IsCorrectSettings)
                _settingsValid = false;
            if (Settings.SettingsErrors.Count > 0)
                Extensions.Log($"🉐 Найстройки некорректные! Список ошибок: " +
                    $"{string.Join("", Settings.SettingsErrors)}", _logWriter);

            else Extensions.Log("🉐 Настройки корректны!", _logWriter);
            Extensions.Log("🉐 Проверка корректности настроек завершена...", _logWriter);
        }

        /// <summary>
        /// Вызывается перед каждым тестом
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            Extensions.Log("🌍 Подготовка перед тестом...", _logWriter);
            if (!_settingsValid)
                Assert.Inconclusive("❌ Настройки некорректны, тесты пропущены.");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Extensions.Log("🧹 Завершение всех тестов.", _logWriter);
            _logWriter?.Dispose();
            if (_logFileDir != null)
                Process.Start("explorer.exe", _logFileDir);
        }

        /// <summary>
        /// Проверяет являются ли все файлы которые должны быть BINI таковыми
        /// </summary>
        [TestMethod]
        public void Bini_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка BINI 🔎\n", _logWriter);
            var biniFilesJson = File.ReadAllText("Configs\\ExpectedBiniFiles.json");
            Assert.IsTrue(!string.IsNullOrEmpty(biniFilesJson));
            var config = JsonConvert.DeserializeObject<BiniConfig>(biniFilesJson);
            Assert.IsTrue(config != null && config.CryptBini.Count > 0);

            bool error = false;
            var nonBiniFiles = new List<string>();
            var nonFiles = new List<string>();
            foreach (var block in config.CryptBini)
            {
                var path = block.Key;
                var files = block.Value;
                foreach (var file in files)
                {
                    var filePath = Path.Combine(Settings.SettingsTestsData.FolderINIS,
                        path, file);
                    // если файл существует проверяем BINI ли он
                    if (File.Exists(filePath))
                    {
                        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        var header = new byte[4];
                        fs.Read(header, 0, 4);
                        string signature = Encoding.ASCII.GetString(header);

                        if (signature != "BINI")
                        {
                            Extensions.Log($"💂‍♂️ {file} - НЕ BINI", _logWriter);
                            nonBiniFiles.Add(file);
                        }
                        else Extensions.Log($"♻️ {file} - BINI", _logWriter);
                    }
                    else
                    {
                        Extensions.Log($"💧 {filePath} - НЕ СУЩЕСТВУЕТ", _logWriter);
                        nonFiles.Add(filePath);
                    }
                }
            }

            if (nonBiniFiles.Count > 0)
                Assert.Fail("Следующие файлы не в BINI-формате:\n" + string.Join("\n", nonBiniFiles));
            else Extensions.Log($"♻️ Все файлы BINI исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверяет наличие анимаций в файлах CMP
        /// </summary>
        [TestMethod]
        public void SearchAnimations_Test()
        {
            if (!Settings.IsCorrectCMPXML) return;
            Extensions.Log("\n\n\t🔎 Проверка Animations CMP 🔎\n", _logWriter);

            var animationsByFile = new Dictionary<string, HashSet<string>>();
            var parser = new AnimationXMLParser(animationsByFile, Settings.SettingsTestsData.FolderCMPSXML);
            parser.Parse();

            // Вывод результата
            Extensions.Log("\n🔍 Найденные анимации и файлы:\n", _logWriter);
            foreach (var kvp in animationsByFile)
            {
                Extensions.Log($"🌀 {kvp.Key}", _logWriter);
                foreach (var file in kvp.Value)
                {
                    Extensions.Log($"   └ 📄 {file}", _logWriter);
                }
            }
        }

        /// <summary>
        /// Проверяет наличие Hardpoints в файлах CMP
        /// </summary>
        [TestMethod]
        public void SearchHardpoints_Test()
        {
            if (!Settings.IsCorrectCMPXML) return;
            Extensions.Log("\n\n\t🔎 Проверка Hardpoints CMP 🔎\n", _logWriter);

            var hardpointsByFile = new Dictionary<string, HashSet<string>>();
            var parser = new HardpointsXMLParser(hardpointsByFile, Settings.SettingsTestsData.FolderCMPSXML);
            parser.Parse();

            Extensions.Log("\n📍 Найденные Hardpoints по файлам:\n", _logWriter);
            foreach (var kvp in hardpointsByFile)
            {
                Extensions.Log($"📄 {Path.GetFileName(kvp.Key)}", _logWriter);
                foreach (var point in kvp.Value)
                {
                    Extensions.Log($"   └ 🎯 {point}", _logWriter);
                }
            }
        }

        /// <summary>
        /// Проверка корректности текстур в STARSPHERE, FX
        /// </summary>
        [TestMethod]
        public void CorrectViewTextures_Test()
        {
            // STARSPHERE
            // генерируем файлы UNIVERSE и файлы порталов  для него, чтобы попасть
            // в системы без объектов и смертельных зон в точку 0 0 0 где будут два портала один красный другой голубой в 2к от игрока
            // задача порталов вести по пути от первой до последней системы вести через окружения
            // оценка систем проводится глазами без лишних помех перед ними на голых сферах


            Assert.Fail("НЕ РЕАЛИЗОВАН");
        }

        /// <summary>
        /// Проверяет use_animation наличие анимаций в файлах CMP
        /// </summary>
        [TestMethod]
        public void ExistAnimations_Test()
        {
            if (!Settings.IsCorrectCMPXML) return;
            Extensions.Log("\n\n\t🔎 Проверка use_animation в Animations CMP 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            var animationsByFile = new Dictionary<string, HashSet<string>>();
            var parser = new AnimationXMLParser(animationsByFile, Settings.SettingsTestsData.FolderCMPSXML);
            parser.Parse();

            // проверка наличия use_animation в папке с CMPXML
            var collection = new ValueCollection();
            var collectionUse = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "use_animation");

            foreach(var use in collectionUse.Result.Keys)
            {
                if(!animationsByFile.ContainsKey(use))
                {
                    Extensions.Log($"  ⭕  {use} не существует | {collectionUse.Result[use][0].LineNumber} | {collectionUse.Result[use][0].FilePath}", _logWriter);
                    errorNotFound.Add($"  ⭕  {use} не существует | {collectionUse.Result[use][0].LineNumber} | {collectionUse.Result[use][0].FilePath}");
                }
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("use_animation не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все use_animation исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверяет CMP на потенциальную возможность анимирования
        /// </summary>
        [TestMethod]
        public void ExistPrisAnimations_Test()
        {
            if (!Settings.IsCorrectCMPXML) return;
            Extensions.Log("\n\n\t🔎 Проверка всех моделей на предмет вращаящихся частей 🔎\n", _logWriter);

            var parser = new AnimationXMLParser(Settings.SettingsTestsData.FolderCMPSXML);
            var result = parser.ParsePrisObjectCMPXML();

            foreach(var anim in result)
            {
                Extensions.Log(anim, _logWriter);
            }
        }

        /// <summary>
        /// Поиск всех material_library которые не существуют
        /// </summary>
        [TestMethod]
        public void ExistMaterialPaths_Test()
        {
            if (!Settings.IsCorrectINI && !Settings.IsCorrect3DB && !Settings.IsCorrectMAT
                && !Settings.IsCorrectTXM && !Settings.IsCorrectCMP) return;
            Extensions.Log("\n\n\t🔎 Проверка material_library 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия material_library в папке с MATS
            var collection = new ValueCollection();
            var collectionMats = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "material_library");
            foreach (var lib in collectionMats.Result)
            {
                var path = "";
                var ext = Path.GetExtension(lib.Key);
                switch (ext)
                {
                    case ".mat":
                        path = Path.Combine(Settings.SettingsTestsData.FolderMATS, "DATA", lib.Key);
                        break;
                    case ".cmp":
                        path = Path.Combine(Settings.SettingsTestsData.FolderCMPS, "DATA", lib.Key);
                        break;
                    case ".txm":
                        path = Path.Combine(Settings.SettingsTestsData.FolderTXMS, "DATA", lib.Key);
                        break;
                    case ".3db":
                        path = Path.Combine(Settings.SettingsTestsData.Folder3DBS, "DATA", lib.Key);
                        break;
                }

                if (!File.Exists(path))
                {
                    errorNotFound.Add($"⭕{lib.Key} - не существует! {lib.Value[0]}\n");
                    Extensions.Log($"⭕{lib.Key} - не существует! {lib.Value[0]} \n", _logWriter);
                }
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("material_library не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все material_library исправны\n", _logWriter);
        }

        /// <summary>
        /// Поиск всех item_icon которые не существуют
        /// </summary>
        [TestMethod]
        public void ExistItemIconPaths_Test()
        {
            if (!Settings.IsCorrectINI && !Settings.IsCorrect3DB && !Settings.IsCorrectMAT
                && !Settings.IsCorrectTXM && !Settings.IsCorrectCMP) return;
            Extensions.Log("\n\n\t🔎 Проверка item_icon 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия item_icon в папке с MATS
            var collection = new ValueCollection();
            var collectionMats = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "item_icon");
            foreach (var lib in collectionMats.Result)
            {
                var path = "";
                var ext = Path.GetExtension(lib.Key);
                switch (ext)
                {
                    case ".cmp":
                        path = Path.Combine(Settings.SettingsTestsData.FolderCMPS, "DATA", lib.Key);
                        break;
                    case ".3db":
                        path = Path.Combine(Settings.SettingsTestsData.Folder3DBS, "DATA", lib.Key);
                        break;
                }

                if (!File.Exists(path))
                {
                    errorNotFound.Add($"⭕{lib.Key} - не существует! {lib.Value[0]}\n");
                    Extensions.Log($"⭕{lib.Key} - не существует! {lib.Value[0]} \n", _logWriter);
                }
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("item_icon не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все item_icon исправны\n", _logWriter);
        }

        /// <summary>
        /// Поиск всех DA_archetype которые не существуют
        /// </summary>
        [TestMethod]
        public void ExistDAArchetypePaths_Test()
        {
            if (!Settings.IsCorrectINI && !Settings.IsCorrect3DB && !Settings.IsCorrectMAT
                && !Settings.IsCorrectTXM && !Settings.IsCorrectCMP && !Settings.IsCorrectSPH) return;
            Extensions.Log("\n\n\t🔎 Проверка DA_archetype 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия material_library в папке с MATS
            var collection = new ValueCollection();
            var collectionMats = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "DA_archetype");
            foreach (var lib in collectionMats.Result)
            {
                var path = "";
                var ext = Path.GetExtension(lib.Key);
                switch (ext)
                {
                    case ".cmp":
                        path = Path.Combine(Settings.SettingsTestsData.FolderCMPS, "DATA", lib.Key);
                        break;
                    case ".3db":
                        path = Path.Combine(Settings.SettingsTestsData.Folder3DBS, "DATA", lib.Key);
                        break;
                    case ".sph":
                        path = Path.Combine(Settings.SettingsTestsData.FolderSPHS, "DATA", lib.Key);
                        break;
                }

                if (!File.Exists(path))
                {
                    errorNotFound.Add($"⭕{lib.Key} - не существует! {lib.Value[0]}\n");
                    Extensions.Log($"⭕{lib.Key} - не существует! {lib.Value[0]} \n", _logWriter);
                }
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("DA_archetype не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все DA_archetype исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить всех информационных карт на наличие 
        /// 
        /// не существует при
        /// ids_info = 0
        /// ids_info = 1
        /// </summary>
        [TestMethod]
        public void InfocardExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка наличия информационной карты ids_info у объекта 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // ids_info проверка пустых заданных значений
            var collection = new ValueCollection();
            var foundsIdsInfo = collection.FindValuesWithOptionalNickname(Settings.SettingsTestsData.FolderINIS,
                new HashSet<string>() { "0", "1" }, searchKey: "ids_info");
            Extensions.Log("\t\t🔍 Пустые ids_info:", _logWriter);

            foreach (var pair in foundsIdsInfo)
            {
                Extensions.Log($"✴️ [{pair.Nickname}][{pair.LineNumber}] -> ids_info = {pair.KeyValue} [FILE::{pair.FilePath}]", _logWriter);
                errorNotFound.Add($"✴️ [{pair.Nickname}][{pair.LineNumber}] -> ids_info = {pair.KeyValue} [FILE::{pair.FilePath}]");
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("Найдены не заданные значения ids_info:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все ids_info исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить все названия ids_name на наличии
        /// 
        /// не существует при
        /// ids_name = 0
        /// ids_name = 1
        /// </summary>
        [TestMethod]
        public void NamesExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка наличия названия ids_name у объекта 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // ids_name проверка пустых заданных значений
            var collection = new ValueCollection();
            var foundsIdsName = collection.FindValuesWithOptionalNickname(Settings.SettingsTestsData.FolderINIS,
                new HashSet<string>() { "0", "1" }, searchKey: "ids_name");

            Extensions.Log("\t\t🔍 Пустые ids_name:", _logWriter);
            foreach (var pair in foundsIdsName)
            {
                Extensions.Log($"🔸 [{pair.Nickname}][{pair.LineNumber}] -> ids_name = {pair.KeyValue} [FILE::{pair.FilePath}]", _logWriter);
                errorNotFound.Add($"🔸 [{pair.Nickname}][{pair.LineNumber}] -> ids_name = {pair.KeyValue} [FILE::{pair.FilePath}]");
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("Найдены не заданные значения ids_name:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все ids_name исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить исправность всех CRC чисел эффектов
        /// </summary>
        [TestMethod]
        public void EffectCRCValids_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка исправности effect_crc у эффекта 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // effect_crc проверка исправности
            var collection = new ValueCollection();
            var values = collection.CollectAllValKeysLibrariesWithLocationWithOtherKey(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "effect_crc");

            Extensions.Log("\t\t🔍 Не верные effect_crc:", _logWriter);
            foreach (var pair in values.Result)
            {
                foreach(var val in pair.Value)
                {
                    var CorrectCRC = CrcTool.FLAleCrc(val.OtherKeyValue.ToLower()).ToString();
                    if(CorrectCRC != pair.Key)
                    {
                        Extensions.Log($"🔸 [{pair.Key}][{val.OtherKeyValue}] -> not correct! Setup:[{CorrectCRC}] [FILE::{val.FilePath}]", _logWriter);
                        errorNotFound.Add($"🔸 [{pair.Key}][{val.OtherKeyValue}] -> not correct! Setup:[{CorrectCRC}] [FILE::{val.FilePath}]");
                    }
                }
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("Найдены не верные значения effect_crc:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все effect_crc исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить исправность всех текстур эффектов (ПУТЕЙ К НИМ)
        /// </summary>
        [TestMethod]
        public void EffectTexturesValids_Test()
        {
            try
            {

                if (!Settings.IsCorrectINI || !Settings.IsCorrectTXM) return;
                Extensions.Log("\n\n\t🔎 Проверка исправности textures путей к эффектам 🔎\n", _logWriter);
                var errorNotFound = new List<string>();

                // effect_crc проверка исправности
                var collection = new ValueCollection();
                var values = collection.CollectAllValKeysLibrariesWithLocationWithOtherKey(Settings.SettingsTestsData.FolderINIS,
                    searchValuesKey: "textures");

                Extensions.Log("\t\t🔍 Не верные пути к текстурам textures в эффектах:", _logWriter);
                foreach (var pair in values.Result)
                {
                    var path = Path.Combine(Settings.SettingsTestsData.FolderTXMS, "DATA", pair.Key);
                    if (!File.Exists(path))
                    {
                        foreach (var val in pair.Value)
                        {
                            if (val.FilePath.Contains("ReShade.ini")) continue;
                            Extensions.Log($"🔸 [{pair.Key}][{val.OtherKeyValue}] -> not exist FILE PATH [FILE::{val.FilePath}]", _logWriter);
                            errorNotFound.Add($"🔸 [{pair.Key}][{val.OtherKeyValue}] -> not exist FILE PATH [FILE::{val.FilePath}]");
                        }
                    }
                }

                if (errorNotFound.Count > 0)
                    Assert.Fail("Найдены не правильные пути до textures:\n" + string.Join("\n", errorNotFound));
                else Extensions.Log($"♻️ Все textures исправны\n", _logWriter);
            }
            catch (Exception ex)
            {
                var msg = ex.Message.ToString();
            }
        }

        #region Key Exist

        /// <summary>
        /// Проверка всех explosion_arch
        /// 
        /// Запись, [Explosion] определяющая эффект взрыва и урон мины. 
        /// Примечание: урон всегда будет сосредоточен в центре мины, а не там, где будет воспроизводиться эффект.
        /// </summary>
        [TestMethod]
        public void ExplosionArchExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка explosion_arch 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия explosion_arch
            var collection = new ValueCollection();
            var values = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "explosion_arch");

            var path1 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
                "DATA\\FX",
                "explosions.ini");
            var path2 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
                "DATA\\EQUIPMENT",
                "weapon_equip.ini");
            var state = collection.GetCollection(new string[] { path1, path2 },
                "explosion", "nickname");

            if (state.State == StateScanCollectionEnum.Success)
            {
                foreach (var valuePair in values.Result)
                {
                    if (!collection.Collection.Contains(valuePair.Key.ToLower()))
                    {
                        errorNotFound.Add($"{valuePair.Key} - не сущестует [{valuePair.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {valuePair.Key} - не сущестует [{valuePair.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"explosions.ini, weapon_equip.ini не найдены! {state.Message}\n");

            if (errorNotFound.Count > 0)
                Assert.Fail("explosion_arch не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все explosion_arch исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверка всех shield_type
        /// 
        /// Запись, [WeaponType] а именно упоминается shield_mod который и является shield_type
        /// Примечание: определяет тип защиты щита на который уже конфигурируется защита от оружия
        /// </summary>
        [TestMethod]
        public void ShieldTypeExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка shield_type 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия shield_type
            var collection = new ValueCollection();
            var values = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "shield_type");

            var pathEffects1 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
                "DATA\\EQUIPMENT",
                "weaponmoddb.ini");
            var effectsState = collection.GetCollection(pathEffects1,
                "WeaponType", "shield_mod", manyKeys: true, index: 0);

            if (effectsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var valuePair in values.Result)
                {
                    if (!collection.Collection.Contains(valuePair.Key.ToLower()))
                    {
                        errorNotFound.Add($"{valuePair.Key} - не сущестует [{valuePair.Value[0]}]\n");
                        Extensions.Log($"⭕ {valuePair.Key} - не сущестует [{valuePair.Value[0]}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"weaponmoddb.ini не найдены! {effectsState.Message}\n");

            if (errorNotFound.Count > 0)
                Assert.Fail("shield_type не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все shield_type исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверка всех debris_type
        /// 
        /// Запись, [Debris] определяющая тип обломков. 
        /// </summary>
        [TestMethod]
        public void DebrisTypeExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка debris_type 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия debris_type
            var collection = new ValueCollection();
            var values = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "debris_type");

            var path1 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
                "DATA\\FX",
                "explosions.ini");
         
            var state = collection.GetCollection(path1,
                "Debris", "nickname", false, index: 0);

            if (state.State == StateScanCollectionEnum.Success)
            {
                foreach (var valuePair in values.Result)
                {
                    var deb = valuePair.Key.Split(",")[0].ToLower();

                    if (!collection.Collection.Contains(deb))
                    {
                        errorNotFound.Add($"{valuePair.Key} - не сущестует [{valuePair.Value[0]}]\n");
                        Extensions.Log($"⭕ {valuePair.Key} - не сущестует [{valuePair.Value[0]}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"explosions.ini не найдены! {state.Message}\n");

            if (errorNotFound.Count > 0)
                Assert.Fail("debris_type не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все debris_type исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверка всех эффектов
        /// 
        /// Запись, [Effect] определяющая эффект. 
        /// </summary>
        [TestMethod]
        public void EffectsExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка эффектов 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия эффектов
            var collection = new ValueCollection();

            // проверить shield_collapse_particle, shield_hit_effects (index:1), particles
            // в effects.ini (FX) к [Effect] в ключ nickname
            Extensions.Log("\t🔍 shield_collapse_particle в effects.ini:\n\n", _logWriter);
            var shield_collapse_particles = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
               searchValuesKey: "shield_collapse_particle");

            var pathEngineEffects = Path.Combine(Settings.SettingsTestsData.FolderINIS,
                "DATA\\FX\\ENGINES",
                "engines_ale.ini");
            var pathEffects = Path.Combine(Settings.SettingsTestsData.FolderINIS,
             "DATA\\FX",
             "effects.ini");
            var effectsState = collection.GetCollection(pathEffects,
                "Effect", "nickname");
            if (effectsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var shield_collapse_particle in shield_collapse_particles.Result)
                {
                    if (!collection.Collection.Contains(shield_collapse_particle.Key.ToLower()))
                    {
                        errorNotFound.Add($"{shield_collapse_particle.Key} - не сущестует в effects.ini[{shield_collapse_particle.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {shield_collapse_particle.Key} - не сущестует в effects.ini[{shield_collapse_particle.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"effects.ini не найден! {effectsState.Message}\n");
            Extensions.Log($"♻️ shield_collapse_particle в effects.ini - проверка завершена\n", _logWriter);

            Extensions.Log("\t🔍 shield_hit_effects в effects.ini:\n\n", _logWriter);
            var shield_hit_effectss = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "shield_hit_effects");

            effectsState = collection.GetCollection(pathEffects,
                "Effect", "nickname");
            if (effectsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var shield_hit_effects in shield_hit_effectss.Result)
                {
                    var val_shield_hit_effects = shield_hit_effects.Key.Split(",")[1].Trim().ToLower();
                    if (!collection.Collection.Contains(val_shield_hit_effects))
                    {
                        errorNotFound.Add($"{shield_hit_effects.Key} - не сущестует в effects.ini[{shield_hit_effects.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {shield_hit_effects.Key} - не сущестует в effects.ini[{shield_hit_effects.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"effects.ini не найден! {effectsState.Message}\n");
            Extensions.Log($"♻️ shield_hit_effects в effects.ini - проверка завершена\n", _logWriter);


            Extensions.Log("\t🔍 particles в effects.ini:\n\n", _logWriter);
            var particless = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "particles");

            effectsState = collection.GetCollection(pathEffects,
                "Effect", "nickname");
            if (effectsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var particle in particless.Result)
                {
                    var val_particle = particle.Key.Trim().ToLower();
                    string val_particle_res = val_particle.Split(';')[0].Trim();//чистим комментарии строки после ;
                    if (!collection.Collection.Contains(val_particle_res))
                    {
                        errorNotFound.Add($"{particle.Key} - не сущестует в effects.ini[{particle.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {particle.Key} - не сущестует в effects.ini[{particle.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"effects.ini не найден! {effectsState.Message}\n");
            Extensions.Log($"♻️ particle в effects.ini - проверка завершена\n", _logWriter);

            #region EnginesEfects

            //flame_effect


            Extensions.Log("\t🔍 flame_effect в effects.ini:\n\n", _logWriter);
            var flames = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "flame_effect");

            var effectsEngineState = collection.GetCollection(pathEngineEffects,
                "VisEffect", "nickname");
            if (effectsEngineState.State == StateScanCollectionEnum.Success)
            {
                foreach (var particle in flames.Result)
                {
                    var val_particle = particle.Key.Trim().ToLower();
                    string val_particle_res = val_particle.Split(';')[0].Trim();//чистим комментарии строки после ;
                    if (!collection.Collection.Contains(val_particle_res))
                    {
                        errorNotFound.Add($"{particle.Key} - не сущестует в engines_ale.ini[{particle.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {particle.Key} - не сущестует в engines_ale.ini[{particle.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"engines_ale.ini не найден! {effectsState.Message}\n");
            Extensions.Log($"♻️ flame_effect в engines_ale.ini - проверка завершена\n", _logWriter);

            Extensions.Log("\t🔍 flame_effect в effects.ini:\n\n", _logWriter);
            flames = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "flame_effect");

            effectsState = collection.GetCollection(pathEffects,
                "Effect", "nickname");
            if (effectsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var particle in flames.Result)
                {
                    var val_particle = particle.Key.Trim().ToLower();
                    string val_particle_res = val_particle.Split(';')[0].Trim();//чистим комментарии строки после ;
                    if (!collection.Collection.Contains(val_particle_res))
                    {
                        errorNotFound.Add($"{particle.Key} - не сущестует в effects.ini[{particle.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {particle.Key} - не сущестует в effects.ini[{particle.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"effects.ini не найден! {effectsState.Message}\n");
            Extensions.Log($"♻️ flame_effect в effects.ini - проверка завершена\n", _logWriter);

            //trail_effect

            Extensions.Log("\t🔍 trail_effect в effects.ini:\n\n", _logWriter);
            var trales = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "trail_effect");

            effectsEngineState = collection.GetCollection(pathEngineEffects,
                "VisEffect", "nickname");
            if (effectsEngineState.State == StateScanCollectionEnum.Success)
            {
                foreach (var particle in trales.Result)
                {
                    var val_particle = particle.Key.Trim().ToLower();
                    string val_particle_res = val_particle.Split(';')[0].Trim();//чистим комментарии строки после ;
                    if (!collection.Collection.Contains(val_particle_res))
                    {
                        errorNotFound.Add($"{particle.Key} - не сущестует в engines_ale.ini[{particle.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {particle.Key} - не сущестует в engines_ale.ini[{particle.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"engines_ale.ini не найден! {effectsState.Message}\n");
            Extensions.Log($"♻️ trail_effect в engines_ale.ini - проверка завершена\n", _logWriter);

            Extensions.Log("\t🔍 trail_effect в effects.ini:\n\n", _logWriter);
            trales = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "trail_effect");

            effectsState = collection.GetCollection(pathEffects,
                "Effect", "nickname");
            if (effectsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var particle in trales.Result)
                {
                    var val_particle = particle.Key.Trim().ToLower();
                    string val_particle_res = val_particle.Split(';')[0].Trim();//чистим комментарии строки после ;
                    if (!collection.Collection.Contains(val_particle_res))
                    {
                        errorNotFound.Add($"{particle.Key} - не сущестует в effects.ini[{particle.Value[0].FilePath}]\n");
                        Extensions.Log($"⭕ {particle.Key} - не сущестует в effects.ini[{particle.Value[0].FilePath}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"effects.ini не найден! {effectsState.Message}\n");
            Extensions.Log($"♻️ trail_effect в effects.ini - проверка завершена\n", _logWriter);

            //trail_effect_player
            //cruise_disrupt_effect

            #endregion

            if (errorNotFound.Count > 0)
                Assert.Fail("Эффекты не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все эффекты исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверка всех звуков
        /// 
        /// Запись, [Sound] определяющая эффект. 
        /// </summary>
        [TestMethod]
        public void SoundsExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка звуков и музыки 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            var collection = new ValueCollection();

            // проверить shield_collapse_sound, shield_rebuilt_sound в sounds.ini (AUDIO) к [Sound] в ключ nickname
            Extensions.Log("\t🔍 shield_collapse_sound в sounds.ini:\n\n", _logWriter);
            var shield_collapse_sounds = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "shield_collapse_sound");

            var pathSounds = Path.Combine(Settings.SettingsTestsData.FolderINIS,
             "DATA\\AUDIO",
             "sounds.ini");
            var soundsState = collection.GetCollection(pathSounds,
                "Sound", "nickname");
            if (soundsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var shield_collapse_sound in shield_collapse_sounds.Result)
                {
                    if (!collection.Collection.Contains(shield_collapse_sound.Key))
                    {
                        errorNotFound.Add($"{shield_collapse_sound.Key} - не сущестует в sounds.ini[{shield_collapse_sound.Value[0]}]\n");
                        Extensions.Log($"⭕ {shield_collapse_sound.Key} - не сущестует в sounds.ini[{shield_collapse_sound.Value[0]}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"sounds.ini не найден! {soundsState.Message}\n");
            Extensions.Log($"♻️ shield_collapse_sound в sounds.ini - проверка завершена\n", _logWriter);

            Extensions.Log("\t🔍 shield_rebuilt_sound в sounds.ini:\n\n", _logWriter);
            var shield_rebuilt_sounds = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "shield_rebuilt_sound");

            soundsState = collection.GetCollection(pathSounds,
                "Sound", "nickname");
            if (soundsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var shield_rebuilt_sound in shield_rebuilt_sounds.Result)
                {
                    if (!collection.Collection.Contains(shield_rebuilt_sound.Key))
                    {
                        errorNotFound.Add($"{shield_rebuilt_sound.Key} - не сущестует в sounds.ini[{shield_rebuilt_sound.Value[0]}]\n");
                        Extensions.Log($"⭕ {shield_rebuilt_sound.Key} - не сущестует в sounds.ini[{shield_rebuilt_sound.Value[0]}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"sounds.ini не найден! {soundsState.Message}\n");
            Extensions.Log($"♻️ shield_rebuilt_sound в sounds.ini - проверка завершена\n", _logWriter);

            Extensions.Log("\t🔍 use_sound в sounds.ini:\n\n", _logWriter);
            var use_sounds = collection.CollectAllValKeysLibrariesWithLocation(Settings.SettingsTestsData.FolderINIS,
                searchValuesKey: "use_sound");

            var pathSounds2 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
               "DATA\\AUDIO",
               "engine_sounds.ini");

            soundsState = collection.GetCollection(new string[] { pathSounds, pathSounds2 },
                "Sound", "nickname");
            if (soundsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var use_sound in use_sounds.Result)
                {
                    if (!collection.Collection.Contains(use_sound.Key))
                    {
                        errorNotFound.Add($"{use_sound.Key} - не сущестует в sounds.ini, engine_sounds.ini[{use_sound.Value[0]}]\n");
                        Extensions.Log($"⭕ {use_sound.Key} - не сущестует в sounds.ini, engine_sounds.ini[{use_sound.Value[0]}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"sounds.ini, engine_sounds.ini не найден! {soundsState.Message}\n");
            Extensions.Log($"♻️ use_sound в sounds.ini, engine_sounds.ini - проверка завершена\n", _logWriter);


            if (errorNotFound.Count > 0)
                Assert.Fail("Звуки и Музыка не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все Звуки и Музыка исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверка всех контейнеров [LootCrate](ящик с добычей) [CargoPod] (грузовые отсеки)
        /// 
        /// Запись, [LootCrate] определяющая ящик с добычей. 
        /// Запись, [CargoPod] определяющая грузовые отсеки. 
        /// </summary>
        [TestMethod]
        public void LootCrateExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка контейнеров 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия эффектов
            var collection = new ValueCollection();

            // проверка loot_appearance, pod_appearance в select_equip.ini из [LootCrate] ключ nickname
            Extensions.Log("\t🔍 loot_appearance в select_equip.ini и др.:\n\n", _logWriter);
            var loot_appearances = collection.CollectAllValKeysLibrariesWithLocation(
                Settings.SettingsTestsData.FolderINIS,
               searchValuesKey: "loot_appearance");

            var path1 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
             "DATA\\EQUIPMENT",
             "select_equip.ini");
            var path2 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
             "DATA\\EQUIPMENT",
             "misc_equip.ini");
            var path3 = Path.Combine(Settings.SettingsTestsData.FolderINIS,
             "DATA\\EQUIPMENT",
             "weapon_equip.ini");
            var state = collection.GetCollection(new string[] {path1, path2, path3},
                "LootCrate", "nickname");
            if (state.State == StateScanCollectionEnum.Success)
            {
                foreach (var loot_appearance in loot_appearances.Result)
                {
                    if (!collection.Collection.Contains(loot_appearance.Key.ToLower()))
                    {
                        errorNotFound.Add($"{loot_appearance.Key} - не сущестует [{loot_appearance.Value[0]}]\n");
                        Extensions.Log($"⭕ {loot_appearance.Key} - не сущестует [{loot_appearance.Value[0]}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"select_equip.ini, misc_equip.ini, weapon_equip.ini не найдены! {state.Message}\n");
            Extensions.Log($"♻️ loot_appearance в select_equip.ini, misc_equip.ini, weapon_equip.ini - проверка завершена\n", _logWriter);

            Extensions.Log("\t🔍 pod_appearance в select_equip.ini и др.:\n\n", _logWriter);
            var pod_appearances = collection.CollectAllValKeysLibrariesWithLocation(
                Settings.SettingsTestsData.FolderINIS,
               searchValuesKey: "pod_appearance");

            state = collection.GetCollection(new string[] { path1, path2, path3 },
                "CargoPod", "nickname");
            if (state.State == StateScanCollectionEnum.Success)
            {
                foreach (var pod_appearance in pod_appearances.Result)
                {
                    if (!collection.Collection.Contains(pod_appearance.Key.ToLower()))
                    {
                        errorNotFound.Add($"{pod_appearance.Key} - не сущестует [{pod_appearance.Value[0]}]\n");
                        Extensions.Log($"⭕ {pod_appearance.Key} - не сущестует [{pod_appearance.Value[0]}]\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"select_equip.ini, misc_equip.ini, weapon_equip.ini не найдены! {state.Message}\n");
            Extensions.Log($"♻️ pod_appearance в select_equip.ini, misc_equip.ini, weapon_equip.ini - проверка завершена\n", _logWriter);

            if (errorNotFound.Count > 0)
                Assert.Fail("Контейнеры не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все контейнеров исправны\n", _logWriter);
        }

        /// <summary>
        /// Тестирование валидности фракции [faction =]
        /// </summary>
        [TestMethod]
        public void FactionExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка валидности фракции 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            // проверка наличия эффектов
            var collection = new ValueCollection();
            var countFactions = 0;

            // получаем список faction
            Extensions.Log("\t🔍 faction в ini:\n\n", _logWriter);
            var factions = collection.CollectAllValKeysLibrariesWithLocation(
                Settings.SettingsTestsData.FolderINIS,
               searchValuesKey: "faction");
            // получаем из initialworld.ini список фракций
            var pathInitialWorld = Path.Combine(Settings.SettingsTestsData.FolderINIS, "DATA", "initialworld.ini");
            var allFactionsState = collection.GetCollection(pathInitialWorld, "Group", "nickname");
            if (allFactionsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var faction in factions.Result)
                {
                    var factionsKey = faction.Key.Split(',');
                    countFactions++;
                    foreach (var fk in factionsKey)
                    {
                        bool checkInt = int.TryParse(fk.Trim(), out var valueInt);
                        bool checkFloat = fk.Trim().Contains(".");
                        if (checkInt || checkFloat || fk == "all") break;
                        if (!collection.Collection.Contains(fk.Trim()))
                        {
                            errorNotFound.Add($"{faction.Key} - [{fk}] - не сущестует в игре");
                            Extensions.Log($"⭕ {faction.Key} - [{fk}] - не сущестует в игре\n", _logWriter);
                            break;
                        }
                    }
                }
            }
            else Assert.Fail($"initialworld.ini не найден! {allFactionsState.Message}\n");
            Extensions.Log($"♻️ faction в ini - проверка завершена\n", _logWriter);

            // получаем список local_faction
            Extensions.Log("\t🔍 local_faction в ini:\n\n", _logWriter);
            factions = collection.CollectAllValKeysLibrariesWithLocation(
                Settings.SettingsTestsData.FolderINIS,
               searchValuesKey: "local_faction");
            // получаем из initialworld.ini список фракций
            allFactionsState = collection.GetCollection(pathInitialWorld, "Group", "nickname");
            if (allFactionsState.State == StateScanCollectionEnum.Success)
            {
                foreach (var faction in factions.Result)
                {
                    var factionsKey = faction.Key.Split(',');
                    countFactions++;
                    foreach (var fk in factionsKey)
                    {
                        bool checkInt = int.TryParse(fk.Trim(), out var valueInt);
                        bool checkFloat = fk.Trim().Contains(".");
                        if (checkInt || checkFloat || fk == "all") break;

                        if (!collection.Collection.Contains(faction.Key))
                        {
                            errorNotFound.Add($"{faction.Key} - не сущестует в игре");
                            Extensions.Log($"⭕ {faction.Key} - не сущестует в игре\n", _logWriter);
                        }
                    }
                }
            }
            else Assert.Fail($"initialworld.ini не найден! {allFactionsState.Message}\n");
            Extensions.Log($"♻️ local_faction в ini - проверка завершена\n", _logWriter);

            if (errorNotFound.Count > 0)
                Assert.Fail($"[{countFactions}] Фракции не существует:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ [{countFactions}] Все фракции исправны\n", _logWriter);
        }

        #endregion
    }
}
