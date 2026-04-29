/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 29 апреля 2026 06:52:26
 * Version: 1.0.20
 */

using System.Diagnostics;

using LibreLancer.Data;
using LibreLancer.Data.Equipment;
using LibreLancer.Data.Ini;

using LizeriumTests.Logic;
using LizeriumTests.Logic.Collections;

/*🔸 [Engine]
    - character_loop_sound
    - character_pitch_range
    - character_start_sound
    - cruise_backfire_sound
    - cruise_charge_time
    - cruise_disrupt_effect
    - cruise_disrupt_sound
    - cruise_loop_sound
    - cruise_power_usage
    - cruise_speed
    - cruise_start_sound
    - cruise_stop_sound
    - DA_archetype
    - engine_kill_sound
    - explosion_arch
    - flac_scan
    - flame_effect
    - hp_mount_group
    - ids_info
    - ids_name
    - indestructible
    - inside_sound_cone
    - linear_drag
    - LODranges
    - lootable
    - mass
    - material_library
    - max_force 
    - nickname
    - outside_cone_attenuation
    - outside_sound_cone
    - power_usage
    - reverse_fraction
    - rumble_atten_range
    - rumble_pitch_range
    - rumble_sound
    - trail_effect
    - trail_effect_player
    - volume
       */

namespace Lizerium.Tests.INI.EQUIPMENT
{
    [TestClass]
    public class EngineEquipTests
    {
        private static StreamWriter? _logWriter;
        private static string? _logFileDir;
        private static string? _logFilePath;
        private bool isInputDataExists { get; set; }
        private static SettingsService Settings { get; set; }
        private static bool _settingsValid = true;

        #region Custom Paramenters

        private static string DIR_FILE_TEST => Path.Combine(Settings.SettingsTestsData.FolderINIS, "DATA\\EQUIPMENT");
        private static string FILE_PATH_TEST => Path.Combine(DIR_FILE_TEST, "engine_equip.ini");
        private static List<string> Lines { get; set; }

        #endregion

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
            _logFilePath = Path.Combine(dirTests, "EngineEquipTests.ini");
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

            if (!_settingsValid)
                Assert.Inconclusive("❌ Настройки некорректны, тесты пропущены.", _logWriter);
            if (!Directory.Exists(DIR_FILE_TEST))
                Assert.Inconclusive($"❌ Настройки некорректны, {DIR_FILE_TEST} - не существует, тесты пропущены.", _logWriter);
            if (!File.Exists(FILE_PATH_TEST))
                Assert.Inconclusive($"❌ Настройки некорректны, {FILE_PATH_TEST} - не существует, тесты пропущены.", _logWriter);

            Lines = [.. File.ReadAllLines(FILE_PATH_TEST)];
        }

        /// <summary>
        /// Вызывается перед каждым тестом
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            Extensions.Log("🌍 Подготовка перед тестом...", _logWriter);
            if (!_settingsValid)
                Assert.Inconclusive("❌ Настройки некорректны, тесты пропущены.", _logWriter);
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
        /// Получить все типы в файле
        /// </summary>
        [TestMethod]
        public void Headers_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка всех известных заговоков 🔎\n", _logWriter);

            var collection = new ValueCollection();
            var headers = collection.GetHeaders(Lines);
            Extensions.Log($"✴️ Все заголовки:\n " + string.Join("\n", headers), _logWriter);
        }

        /// <summary>
        /// Получить все уникальный ключи каждоого блока с данными в файле
        /// </summary>
        [TestMethod]
        public void Fields_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка всех полей блоков файла 🔎\n", _logWriter);

            var collection = new ValueCollection();
            var fieldsPerSection = collection.GetUniqueFieldsPerSection(Lines);
            Extensions.Log("🔍 Уникальные поля по секциям:", _logWriter);
            foreach (var kvp in fieldsPerSection)
            {
                Extensions.Log($"\n🔸 [{kvp.Key}]", _logWriter);
                foreach (var field in kvp.Value.OrderBy(v => v))
                {
                    Extensions.Log($"  - {field}", _logWriter);
                }
            }
        }

        /// <summary>
        /// Проверить исправность переменных ids_name и ids_info в DLL-ресурсах
        /// </summary>
        [TestMethod]
        public void ValidValues_Test()
        {
            if (!Settings.IsCorrectINI || !Settings.IsCorrectDLL)
            {
                Assert.Inconclusive("INI or DLL config invalid");
                return;
            }

            Extensions.Log("\n\n\t🔎 Проверка исправности переменных 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            var path = Path.Combine(Settings.SettingsTestsData.FolderINIS, "DATA", "EQUIPMENT", "engine_equip.ini");

            var errors = new List<string>();
            var originalOut = Console.Out;
            List<Engine> engines = new List<Engine>();
            List<Section> iniEngines = new List<Section>();

            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                using (var stream = new MemoryStream())
                {
                    using (Stream file = File.OpenRead(path))
                    {
                        file.CopyTo(stream);
                    }
                    iniEngines = IniFile.ParseFile(path, stream).ToList();
                }

                // Парсим INI
                foreach (var iniEngine in iniEngines)
                {
                    bool isParse = Engine.TryParse(iniEngine, out var engine);
                    if (isParse && engine != null)
                        engines.Add(engine);
                }

                Console.Out.Flush();
                var output = writer.ToString();
                Console.SetOut(originalOut);

                var lines = output.Split('\n');
                foreach (var line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                        errors.Add(line.Trim());
                }

                if (errors.Any())
                {
                    Extensions.Log("Warnings found:\n" + string.Join("\n", errors), _logWriter);
                    Assert.Fail("Warnings found:\n" + string.Join("\n", errors));
                }

                //check infocards
                LibreLancer.Data.IO.FileSystem VfsOne = LibreLancer.Data.IO.FileSystem.FromPath(Settings.SettingsTestsData.FolderDLLS);
                FreelancerIni iniOne = new FreelancerIni(VfsOne);
                var infocards = new InfocardManager(iniOne.Resources);
                /* Нет даже
                   ids_name = 0
                   ids_info = 0
                */
                var errorIdsNameEngines = engines.Where(it => !infocards.StringIds.Contains(it.IdsName));
                var errorIdsInfoEngines = engines.Where(it => !infocards.InfocardIds.ToList().Contains(it.IdsInfo) && it.IdsInfo != 0);

                if (errorIdsNameEngines.Any())
                {
                    Extensions.Log("error IdsName Engines found:\n" + string.Join("\n", errorIdsNameEngines.Select(it => it.Nickname)), _logWriter);
                    Assert.Fail("error IdsName Engines found:\n" + string.Join("\n", errorIdsNameEngines.Select(it => it.Nickname)));
                }
                if (errorIdsInfoEngines.Any())
                {
                    Extensions.Log("error IdsInfo Engines found:\n" + string.Join("\n", errorIdsInfoEngines.Select(it => it.Nickname)), _logWriter);
                    Assert.Fail("error IdsInfo Engines found:\n" + string.Join("\n", errorIdsInfoEngines.Select(it => it.Nickname)));
                }
            }

            if (engines.Count < 1089)
                errorNotFound.Add($"{engines.Count} != 1089");

            if (errorNotFound.Count > 0)
                Assert.Fail("Переменные имеют следующие ошибки:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все переменные - исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить, что каждая секция [Engine] успешно преобразуется в объект Engine через TryParse
        /// </summary>
        [TestMethod]
        public void ParseSections_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что во всех секциях присутствуют обязательные поля (nickname, ids_name, mass и другие критичные значения)
        /// </summary>
        [TestMethod]
        public void RequiredFields_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что обязательные строковые поля (например nickname) не пустые и не состоят только из пробелов
        /// </summary>
        [TestMethod]
        public void RequiredStringFields_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что в секциях используются только допустимые поля без неизвестных, лишних или опечатанных ключей
        /// </summary>
        [TestMethod]
        public void AllowedFieldsOnly_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что все nickname двигателей уникальны и не повторяются между секциями
        /// </summary>
        [TestMethod]
        public void UniqueNicknames_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что числовые параметры двигателя (mass, max_force, linear_drag, power_usage и другие) находятся в допустимых диапазонах
        /// </summary>
        [TestMethod]
        public void NumericRanges_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что значение reverse_fraction находится в допустимом диапазоне и не нарушает физическую логику двигателя
        /// </summary>
        [TestMethod]
        public void ReverseFractionRange_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что cruise-параметры (cruise_charge_time, cruise_power_usage, cruise_speed) заданы корректно и не содержат аномальных значений
        /// </summary>
        [TestMethod]
        public void CruiseParameters_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить корректность формата диапазонов значений, таких как character_pitch_range, rumble_atten_range и rumble_pitch_range
        /// </summary>
        [TestMethod]
        public void RangeFormat_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить логическую согласованность зависимых полей двигателя (например trail_effect_player без trail_effect, или неполные наборы параметров)
        /// </summary>
        [TestMethod]
        public void FieldDependencies_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить логическую согласованность звуковых параметров двигателя (например наличие loop-звука при наличии start-звука)
        /// </summary>
        [TestMethod]
        public void SoundConsistency_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что в файле отсутствуют пустые, повреждённые или неполные секции [Engine]
        /// </summary>
        [TestMethod]
        public void EmptySections_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }

        /// <summary>
        /// Проверить, что значения volume и power_usage не содержат аномалий, таких как отрицательные или подозрительные значения
        /// </summary>
        [TestMethod]
        public void ResourceUsageSanity_Test()
        {
            Assert.Inconclusive("Not implemented yet");
        }
    }
}
