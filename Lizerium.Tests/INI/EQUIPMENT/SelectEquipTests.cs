/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 05 мая 2026 07:01:24
 * Version: 1.0.26
 */

using System.Diagnostics;

using LizeriumTests.Logic;
using LizeriumTests.Logic.Collections;

namespace Lizerium.Tests.INI.EQUIPMENT
{
    [TestClass]
    public class SelectEquipTests
    {
        private static StreamWriter? _logWriter;
        private static string? _logFileDir;
        private static string? _logFilePath;
        private bool isInputDataExists { get; set; }
        private static SettingsService Settings { get; set; }
        private static bool _settingsValid = true;

        #region Custom Paramenters

        private static string DIR_FILE_TEST => Path.Combine(Settings.SettingsTestsData.FolderINIS, "DATA\\EQUIPMENT");
        private static string FILE_PATH_TEST => Path.Combine(DIR_FILE_TEST, "select_equip.ini");
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
            _logFilePath = Path.Combine(dirTests, "SelectEquipTests.ini");
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
        /// Получить все уникальные ключи каждого блока с данными в файле
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
        /// Проверить ShieldGenerator
        /// </summary>
        [TestMethod]
        public void ShieldGeneratorValues_Test()
        {
            if (!Settings.IsCorrectINI && !Settings.IsCorrect3DB) return;
            Extensions.Log("\n\n\t🔎 Проверка ShieldGenerator 🔎\n", _logWriter);
            var errorNotFound = new List<string>();

            Assert.Inconclusive("Not implemented yet");
            if (errorNotFound.Count > 0)
                Assert.Fail("ShieldGenerator имеет следующие ошибки:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все ShieldGenerator - исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить Armor
        /// </summary>
        [TestMethod]
        public void ArmorValues_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка Armor 🔎\n", _logWriter);
            var errorNotFound = new List<string>();
            Assert.Inconclusive("Not implemented yet");

            if (errorNotFound.Count > 0)
                Assert.Fail("Armor имеет следующие ошибки:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все Armor - исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить AttachedFX - use_throttle
        /// </summary>
        [TestMethod]
        public void AttachedFXValues_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Extensions.Log("\n\n\t🔎 Проверка AttachedFX 🔎\n", _logWriter);
            var errorNotFound = new List<string>();
           
            // проверить тип use_throttle - bool
            var collection = new ValueCollection();
            collection.GetCollection(Lines, "AttachedFX", "use_throttle");
            var throttles = new List<string>();
            throttles.AddRange(collection.Collection);

            foreach(var throttle in throttles)
            {
                var state = bool.TryParse(throttle, out var value);
                if(!state)
                {
                    Extensions.Log($"⭕ {throttle} - не исправен\n", _logWriter);
                    errorNotFound.Add($"{throttle} - не исправен\n");
                }
            }

            if (errorNotFound.Count > 0)
                Assert.Fail("AttachedFX имеет следующие ошибки:\n" + string.Join("\n", errorNotFound));
            else Extensions.Log($"♻️ Все AttachedFX - исправны\n", _logWriter);
        }

        /// <summary>
        /// Проверить InternalFX
        /// </summary>
        [TestMethod]
        public void InternalFXValues_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Assert.Inconclusive("Not implemented yet");
            Extensions.Log("\n\n\t🔎 Проверка InternalFX 🔎\n", _logWriter);
        }

        /// <summary>
        /// Проверить Commodity
        /// </summary>
        [TestMethod]
        public void CommodityValues_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Assert.Inconclusive("Not implemented yet");
            Extensions.Log("\n\n\t🔎 Проверка Commodity 🔎\n", _logWriter);
        }

        /// <summary>
        /// Проверить CargoPod
        /// </summary>
        [TestMethod]
        public void CargoPodValues_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Assert.Inconclusive("Not implemented yet");
            Extensions.Log("\n\n\t🔎 Проверка CargoPod 🔎\n", _logWriter);
        }

        /// <summary>
        /// Проверить Shield
        /// </summary>
        [TestMethod]
        public void ShieldValues_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            Assert.Inconclusive("Not implemented yet");
            Extensions.Log("\n\n\t🔎 Проверка Shield 🔎\n", _logWriter);
        }
    }
}
