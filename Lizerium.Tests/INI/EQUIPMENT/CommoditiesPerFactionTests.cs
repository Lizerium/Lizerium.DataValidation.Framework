/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 апреля 2026 14:25:50
 * Version: 1.0.19
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using LizeriumTests.Logic;
using LizeriumTests.Logic.Collections;

namespace Lizerium.Tests.INI.EQUIPMENT
{
    [TestClass]
    public class CommoditiesPerFactionTests
    {
        private static StreamWriter? _logWriter;
        private static string? _logFileDir;
        private static string? _logFilePath;
        private bool isInputDataExists { get; set; }
        private static SettingsService Settings { get; set; }
        private static bool _settingsValid = true;

        #region Custom Paramenters

        private static string DIR_FILE_TEST => Path.Combine(Settings.SettingsTestsData.FolderINIS, "DATA\\EQUIPMENT");
        private static string FILE_PATH_TEST => Path.Combine(DIR_FILE_TEST, "commodities_per_faction.ini");
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
            _logFilePath = Path.Combine(dirTests, "CommoditiesPerFactionTests.ini");
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
        /// Проверяет исправность введённых типов данных в каждом поле файла
        /// </summary>
        [TestMethod]
        public void ValidTypes_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка введённых типов данных в каждом поле файла 🔎\n", _logWriter);

            var errorNotFound = new List<string>();

            // анализурем типы коллекции строк
            // faction - string
            // MarketGood - string, int, int
            var collection = new ValueCollection();
            collection.GetCollection(Lines, "FactionGood", "faction");
            var factions = new List<string>();
            factions.AddRange(collection.Collection);
            collection.GetCollection(Lines, "FactionGood", "MarketGood", manyKeys: true);
            var MarketGoods = new List<string>();
            MarketGoods.AddRange(collection.Collection);

            foreach(var faction in factions)
            {
                if(string.IsNullOrEmpty(faction))
                    errorNotFound.Add($"{faction} - пуст");
            }
            foreach (var good in MarketGoods)
            {
                var vals = good.Split(",");
                if(vals.Length > 0)
                {
                    if(vals.Length != 3)
                    {
                        Extensions.Log($"⭕ {good} - не исправен | количество элементов {vals.Length}!=3\n", _logWriter);
                        errorNotFound.Add($"{good} - не исправен | количество элементов {vals.Length}!=3");
                    }
                    else
                    {
                        var nickname = vals[0];
                        var val1State = int.TryParse(vals[1], out int res1);
                        var val2State = int.TryParse(vals[2], out int res2);

                        if(!val1State)
                        {
                            Extensions.Log($"⭕ {good} - не исправен | второй элемент[{vals[1]}] - не верный тип\n", _logWriter);
                            errorNotFound.Add($"{good} - не исправен | второй элемент[{vals[1]}] - не верный тип");
                        }
                        if(!val2State)
                        {
                            Extensions.Log($"⭕ {good} - не исправен | третий элемент[{vals[2]}] - не верный тип\n", _logWriter);
                            errorNotFound.Add($"{good} - не исправен | третий элемент[{vals[2]}] - не верный тип");
                        }
                    }
                }
                else
                {
                    Extensions.Log($"⭕ {good} - не исправен\n", _logWriter);
                    errorNotFound.Add($"{good} - не исправен");
                }
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("Ошибки в типах данных в игре:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Ошибок в записях блоков не существует!\n", _logWriter);
        }

        /// <summary>
        /// Проверяет значения commodity в полях MarketGood в FactionGood
        /// </summary>
        [TestMethod]
        public void ExistCommodityMarketGoodToFactionGood_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка commodity в полях MarketGood в FactionGood 🔎\n", _logWriter);

            // получаем из commodities_per_faction.ini список первого значения в MarketGood
            var errorNotFound = new List<string>();
            var collection = new ValueCollection();
            collection.GetCollection(Lines, "FactionGood", "MarketGood", index: 0);
            var marketGoods = new List<string>();
            marketGoods.AddRange(collection.Collection);

            // получаем из select_equip.ini список первого значения в Commodity
            var pathSelectEquip = Path.Combine(Settings.SettingsTestsData.FolderINIS, 
                "DATA\\EQUIPMENT", 
                "select_equip.ini");
            var allCommoditiesState = collection.GetCollection(pathSelectEquip, 
                "Commodity", "nickname");

            if (allCommoditiesState.State == StateScanCollectionEnum.Success)
            {
                foreach (var good in marketGoods)
                {
                    if (!collection.Collection.Contains(good))
                    {
                        errorNotFound.Add($"{good} - не сущестует в игре");
                        Extensions.Log($"⭕ {good} - не сущестует в игре\n", _logWriter);
                    }
                }
            }
            else Assert.Fail($"select_equip.ini не найден! {allCommoditiesState.Message}\n");

            if (errorNotFound.Count > 0)
                Assert.Fail("Товаров нет в игре:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все указанные товары - сущестуют в игре\n", _logWriter);
        }
    }
}
