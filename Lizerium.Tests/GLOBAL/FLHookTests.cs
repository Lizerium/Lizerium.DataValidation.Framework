using System.Diagnostics;

using LizeriumTests.Logic;

namespace Lizerium.Tests.GLOBAL
{
    [TestClass]
    public class FLHookTests
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
            _logFilePath = Path.Combine(dirTests, "FLHookTests.ini");
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
        /// Проверить есть ли в конфигурции регенерации брони все Armor
        /// </summary>
        [TestMethod]
        public void ArmorFLHookConfigExists_Test()
        {
            if (!Settings.IsCorrectINI)
            {
                Assert.Inconclusive("INI config invalid");
                return;
            }
            //Extensions.Log("\n\n\t🔎 Проверка известной брони в конфигурации для регенерации брони 🔎\n", _logWriter);
            //var errorNotFound = new List<string>();

            ////получаем всю броню чтобы сверить существование id брони в конфиге FLHook адрес которого тоже надо передать
            //var collection = new ValueCollection();
            //collection.GetCollection(Lines, "Armor", "nickname");
            //var armors = new List<string>();
            //armors.AddRange(collection.Collection);

            ////убираем те что в маркете не продаются



            //var pathConfigArmorsFLHook = "F:\\0_РАЗОБРАТЬ ИЗОБРЕТЕНИЯ\\LizeriumModConfigsBackup\\ConfigsBackup\\Lizerium\\99.3.1\\_FLHOOK_PLUGINS\\regarmour.ini";
            //var keysInIni = collection.GetArmourKeysFromFile(pathConfigArmorsFLHook, "Armour");
            //// Сравниваем: какие из armors не представлены как ключи
            //var missingArmors = armors.Where(a => !keysInIni.Contains(a)).ToList();

            //Extensions.Log("🔻 Отсутствующие ключи в [Armour]:", _logWriter);
            //foreach (var missing in missingArmors)
            //{
            //    Extensions.Log($"- {missing}", _logWriter);
            //    errorNotFound.Add(missing);
            //}
            Assert.Fail("Не сделан");
            //if (errorNotFound.Count > 0)
            //    Assert.Fail("Отсутствующие ключи в [Armour]:\n" + string.Join("\n", errorNotFound));
        }
    }
}
