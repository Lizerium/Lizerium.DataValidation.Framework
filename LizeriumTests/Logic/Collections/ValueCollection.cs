/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 03 мая 2026 06:52:22
 * Version: 1.0.24
 */


using LizeriumTests.Components.Collections;

namespace LizeriumTests.Logic.Collections
{
    /// <summary>
    /// Сбор данных о commodity
    /// </summary>
    public class ValueCollection : IFreelancerCollection
    {
        public List<string> Collection { get; set; } = new List<string>();

        /// <summary>
        /// Базовый поиск по коллекции используя заголовок Header и ключ по файлу
        /// 
        /// Ищет одно совпадение по ключу в каждом блоке
        /// </summary>
        /// <param name="file">Адрес до файла</param>
        /// <param name="nameHeader">Header - блока INI</param>
        /// <param name="fileKey">Ключ в блоке INI</param>
        /// <returns>StateScanCollection</returns>
        public StateScanCollection GetCollection(string file, string nameHeader, string fileKey)
        {
            Collection.Clear();

            if (string.IsNullOrEmpty(file)) return new StateScanCollection(StateScanCollectionEnum.Error, this, $"{file} is not defined");
            if (!File.Exists(file)) return new StateScanCollection(StateScanCollectionEnum.Error, this, $"{file} is not exist");

            var lines = File.ReadAllLines(file);
            var isSearch = false;
            var value = string.Empty;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var check = line.ToLower();
                nameHeader = nameHeader.ToLower();
                fileKey = fileKey.ToLower();

                //пропуск других тегов
                if (!check.Contains($"[{nameHeader}]")
                    && check.Contains("[")
                    && check.Contains("]"))
                {
                    isSearch = false;
                }

                if (check.Contains($"[{nameHeader}]"))
                {
                    if (!Collection.Contains(value) && !string.IsNullOrEmpty(value))
                        Collection.Add(value);  
                    value = string.Empty;
                    isSearch = true;
                }

                if (isSearch)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (!line.Contains("=")) continue;
                    if (line.StartsWith(";")) continue;

                    var KeyVal = Extensions.KeyValueConvert(line);
                    var Key = KeyVal[0].ToLower().Trim();
                    var Val = KeyVal[1].ToLower().Trim();

                    if (Key == fileKey)
                        value = Val;
                }
            }
            if (!Collection.Contains(value) && !string.IsNullOrEmpty(value))
                Collection.Add(value);
            return new StateScanCollection();
        }

        /// <summary>
        /// Базовый поиск по коллекции используя заголовок Header и ключ по файлу
        /// Ищет одно совпадение по ключу в каждом блоке
        /// Работает с несколькими файлами
        /// </summary>
        /// <param name="files">Массив путей к файлам</param>
        /// <param name="nameHeader">Имя блока INI</param>
        /// <param name="fileKey">Ключ в блоке</param>
        /// <returns>StateScanCollection</returns>
        public StateScanCollection GetCollection(string[] files, string nameHeader, string fileKey)
        {
            Collection.Clear();

            if (files == null || files.Length == 0)
                return new StateScanCollection(StateScanCollectionEnum.Error, this, $"No files provided");

            nameHeader = nameHeader.ToLower();
            fileKey = fileKey.ToLower();

            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(file))
                    continue;

                if (!File.Exists(file))
                    continue;

                var lines = File.ReadAllLines(file);
                var isSearch = false;
                var value = string.Empty;

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var check = line.ToLower();

                    // Пропуск других заголовков
                    if (!check.Contains($"[{nameHeader}]")
                        && check.Contains("[")
                        && check.Contains("]"))
                    {
                        isSearch = false;
                    }

                    if (check.Contains($"[{nameHeader}]"))
                    {
                        if (!string.IsNullOrEmpty(value) && !Collection.Contains(value))
                            Collection.Add(value);

                        value = string.Empty;
                        isSearch = true;
                    }

                    if (isSearch)
                    {
                        if (!line.Contains("=")) continue;
                        if (line.StartsWith(";")) continue;

                        var keyVal = Extensions.KeyValueConvert(line);
                        if (keyVal.Count < 2) continue;

                        var key = keyVal[0].ToLower().Trim();
                        var val = keyVal[1].ToLower().Trim();

                        if (key == fileKey)
                            value = val;
                    }
                }

                if (!string.IsNullOrEmpty(value) && !Collection.Contains(value))
                    Collection.Add(value);
            }

            return new StateScanCollection();
        }

        /// <summary>
        /// Базовый поиск по коллекции используя заголовок Header и ключ по загруженным данным Lines
        /// 
        /// Ищет одно совпадение по ключу в каждом блоке
        /// </summary>
        /// <param name="lines">Сгружнные строки файла</param>
        /// <param name="nameHeader">Header - блока INI</param>
        /// <param name="fileKey">Ключ в блоке INI</param>
        /// <returns>StateScanCollection</returns>
        public StateScanCollection GetCollection(List<string> lines, string nameHeader, string fileKey)
        {
            Collection.Clear();

            var isSearch = false;
            var value = string.Empty;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var check = line.ToLower();
                nameHeader = nameHeader.ToLower();
                fileKey = fileKey.ToLower();

                //пропуск других тегов
                if (!check.Contains($"[{nameHeader}]")
                    && check.Contains("[")
                    && check.Contains("]"))
                {
                    isSearch = false;
                }

                if (check.Contains($"[{nameHeader}]"))
                {
                    if (!Collection.Contains(value) && !string.IsNullOrEmpty(value))
                        Collection.Add(value);
                    value = string.Empty;
                    isSearch = true;
                }

                if (isSearch)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (!line.Contains("=")) continue;
                    if (line.StartsWith(";")) continue;

                    var KeyVal = Extensions.KeyValueConvert(line);
                    var Key = KeyVal[0].ToLower().Trim();
                    var Val = KeyVal[1].ToLower().Trim();

                    if (Key == fileKey)
                        value = Val;
                }
            }
            if (!Collection.Contains(value) && !string.IsNullOrEmpty(value))
                Collection.Add(value);
            return new StateScanCollection();
        }

        /// <summary>
        /// Базовый поиск по коллекции используя заголовок Header и ключ по загруженным данным Lines
        /// 
        /// Ищет сколько угодно ключей в каждом блоке
        /// </summary>
        /// <param name="lines">Сгружнные строки файла</param>
        /// <param name="nameHeader">Header - блока INI</param>
        /// <param name="fileKey">Ключ в блоке INI</param>
        /// <returns>StateScanCollection</returns>
        public StateScanCollection GetCollection(List<string> lines, string nameHeader, 
            string fileKey, bool manyKeys)
        {
            Collection.Clear();

            var isSearch = false;
            var values = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var check = line.ToLower();
                nameHeader = nameHeader.ToLower();
                fileKey = fileKey.ToLower();

                //пропуск других тегов
                if (!check.Contains($"[{nameHeader}]")
                    && check.Contains("[")
                    && check.Contains("]"))
                {
                    isSearch = false;
                }

                if (check.Contains($"[{nameHeader}]"))
                {
                    if (values.Count > 0)
                        Collection.AddRange(values);
                    values.Clear();
                    isSearch = true;
                }

                if (isSearch)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (!line.Contains("=")) continue;
                    if (line.StartsWith(";")) continue;

                    var KeyVal = Extensions.KeyValueConvert(line);
                    var Key = KeyVal[0].ToLower().Trim();
                    var Val = KeyVal[1].ToLower().Trim();

                    if (Key == fileKey)
                        values.Add(Val);
                }
            }
            if (values.Count > 0)
                Collection.AddRange(values);
            return new StateScanCollection();
        }

        /// <summary>
        /// Базовый поиск по коллекции используя заголовок Header и ключ по загруженным данным Lines
        /// 
        /// Ищет сколько угодно ключей в каждом блоке
        /// </summary>
        /// <param name="lines">Сгружнные строки файла</param>
        /// <param name="nameHeader">Header - блока INI</param>
        /// <param name="fileKey">Ключ в блоке INI</param>
        /// <returns>StateScanCollection</returns>
        public StateScanCollection GetCollection(string file, string nameHeader,
            string fileKey, bool manyKeys, int index = -1)
        {
            Collection.Clear();

            if (string.IsNullOrEmpty(file)) return new StateScanCollection(StateScanCollectionEnum.Error, this, $"{file} is not defined");
            if (!File.Exists(file)) return new StateScanCollection(StateScanCollectionEnum.Error, this, $"{file} is not exist");

            var lines = File.ReadAllLines(file);
            var isSearch = false;
            var values = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line == "nickname = W_Npc_2.0")
                {
                    var wa = ";";
                }
                var check = line.ToLower();
                nameHeader = nameHeader.ToLower();
                fileKey = fileKey.ToLower();

                //пропуск других тегов
                if (!check.Contains($"[{nameHeader}]")
                    && check.Contains("[")
                    && check.Contains("]"))
                {
                    isSearch = false;
                }

                if (check.Contains($"[{nameHeader}]"))
                {
                    if (values.Count > 0)
                        Collection.AddRange(values);
                    values.Clear();
                    isSearch = true;
                }

                if (isSearch)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (!line.Contains("=")) continue;
                    if (line.StartsWith(";")) continue;

                    var KeyVal = Extensions.KeyValueConvert(line);
                    var Key = KeyVal[0].ToLower().Trim();
                    var Val = KeyVal[1].ToLower().Trim();

                    if (Key == fileKey)
                    {
                        if(index >= 0)
                        {
                            var data = Val.Split(",");
                            if(data.Length >= index) 
                                values.Add(data[index]);
                        }
                        else values.Add(Val);
                    }
                }
            }
            if (values.Count > 0)
                Collection.AddRange(values);
            return new StateScanCollection();
        }

        /// <summary>
        /// Поиск по коллекции используя заголовок Header и ключ по загруженным данным Lines
        /// поля типа fileKey = val1, val2, val3 где index порядковый номер искомого значения в ключе
        /// 
        /// Ищет сколько угодно ключей с совпадением в index в каждом блоке
        /// </summary>
        /// <param name="lines">Сгружнные строки файла</param>
        /// <param name="nameHeader">Header - блока INI</param>
        /// <param name="fileKey">Ключ в блоке INI</param>
        /// <param name="index">Порядковый номер искомого значения в ключе</param>
        /// <returns>StateScanCollection</returns>
        public StateScanCollection GetCollection(List<string> lines, string nameHeader, 
            string fileKey, int index)
        {
            Collection.Clear();

            var isSearch = false;
            var values = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var check = line.ToLower();
                nameHeader = nameHeader.ToLower();
                fileKey = fileKey.ToLower();

                //пропуск других тегов
                if (!check.Contains($"[{nameHeader}]")
                    && check.Contains("[")
                    && check.Contains("]"))
                {
                    isSearch = false;
                }

                if (check.Contains($"[{nameHeader}]"))
                {
                    if (values.Count > 0)
                        Collection.AddRange(values);
                    values.Clear();
                    isSearch = true;
                }

                if (isSearch)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (!line.Contains("=")) continue;
                    if (line.StartsWith(";")) continue;

                    var KeyVal = Extensions.KeyValueConvert(line);
                    var Key = KeyVal[0].ToLower().Trim();
                    var Val = KeyVal[1].ToLower().Trim();

                    if (Key == fileKey)
                    {
                        var data = Val.Split(',');
                        if (data.Length > 0)
                            values.Add(data[index]);
                    }
                }
            }
            if (values.Count > 0)
                Collection.AddRange(values);
            return new StateScanCollection();
        }

        /// <summary>
        /// Получаем список уникальных заголовков типа [Header]
        /// </summary>
        /// <param name="lines"></param>
        public List<string> GetHeaders(List<string> lines)
        {
            var headers = new List<string>();   
            foreach (var line in lines)
            {
                if(line.Contains("[") && line.Contains("]")
                    && !headers.Contains(line))
                    headers.Add(line);
            }
            return headers;
        }

        /// <summary>
        /// Получает список уникальных ключей каждого блока файла
        /// </summary>
        /// <param name="lines">Массив строк файла</param>
        /// <returns>Dictionary<string, HashSet<string>></returns>
        public Dictionary<string, HashSet<string>> GetUniqueFieldsPerSection(IList<string> lines)
        {
            var result = new Dictionary<string, HashSet<string>>();
            string? currentHeader = null;

            foreach (var line in lines)
            {
                var trimmed = line.Trim();

                // Пропускаем пустые строки и комментарии
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                    continue;

                // Если строка — заголовок секции
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentHeader = trimmed.Trim('[', ']');
                    if (!result.ContainsKey(currentHeader))
                        result[currentHeader] = new HashSet<string>();
                    continue;
                }

                // Если строка внутри секции
                if (currentHeader != null)
                {
                    var equalIndex = trimmed.IndexOf('=');
                    if (equalIndex > 0)
                    {
                        var key = trimmed.Substring(0, equalIndex).Trim();
                        result[currentHeader].Add(key);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Ищет все ключи со значениями во всех INI-файлах в папке (рекурсивно) с указанием файла и номера строки.
        /// </summary>
        public ResultSearchCollection CollectAllValKeysLibrariesWithLocation(string iniRootDir, 
            string searchValuesKey = "material_library")
        {
            try
            {
                var result = new ResultSearchCollection();;

                foreach (var file in Directory.EnumerateFiles(iniRootDir, "*.ini", SearchOption.AllDirectories))
                {
                    int lineNumber = 0;
                    foreach (var line in File.ReadLines(file))
                    {
                        lineNumber++;
                        var trimmed = line.Trim();

                        if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                            continue;

                        if (trimmed.StartsWith(searchValuesKey, StringComparison.OrdinalIgnoreCase))
                        {
                            var eqIndex = trimmed.IndexOf('=');
                            if (eqIndex > 0)
                            {
                                var value = trimmed.Substring(eqIndex + 1).Trim();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (!result.Result.TryGetValue(value, out var list))
                                    {
                                        list = new List<ResultSearchItem>();
                                        result.Result[value] = list;
                                    }
                                    list.Add(new ResultSearchItem(file, lineNumber));
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }
        }


        /// <summary>
        /// Ищет все ключи со значениями во всех INI-файлах в папке (рекурсивно) с указанием файла и номера строки 
        /// + сохраняет дополнительный ключ блока в котором значение было найдено например nickname.
        /// </summary>
        public ResultSearchCollection CollectAllValKeysLibrariesWithLocationWithOtherKey(string iniRootDir,
            string searchValuesKey = "material_library", string otherKey = "nickname")
        {
            try
            {
                var result = new ResultSearchCollection(); ;

                foreach (var file in Directory.EnumerateFiles(iniRootDir, "*.ini", SearchOption.AllDirectories))
                {
                    var otherKeyVal = "";

                    int lineNumber = 0;
                    foreach (var line in File.ReadLines(file))
                    {
                        lineNumber++;
                        var trimmed = line.Trim();

                        if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                            continue;

                        if(trimmed.StartsWith(otherKey, StringComparison.OrdinalIgnoreCase))
                        {
                            var eqIndex = trimmed.IndexOf('=');
                            if (eqIndex > 0)
                            {
                                var value = trimmed.Substring(eqIndex + 1).Trim();
                                otherKeyVal = value;
                            }
                        }

                        if (trimmed.StartsWith(searchValuesKey, StringComparison.OrdinalIgnoreCase))
                        {
                            var eqIndex = trimmed.IndexOf('=');
                            if (eqIndex > 0)
                            {
                                var value = trimmed.Substring(eqIndex + 1).Trim();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (!result.Result.TryGetValue(value, out var list))
                                    {
                                        list = new List<ResultSearchItem>();
                                        result.Result[value] = list;
                                    }
                                    list.Add(new ResultSearchItem(file, lineNumber, otherKeyVal));
                                    otherKeyVal = string.Empty;
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Ищет все значения указанных ключей во всех INI-файлах в папке (рекурсивно),
        /// указывая файл и номер строки.
        /// </summary>
        public Dictionary<string, List<(string filePath, int lineNumber)>> CollectAllValKeysLibrariesWithLocation(
            string iniRootDir,
            params string[] searchValuesKeys)
        {
            var result = new Dictionary<string, List<(string filePath, int lineNumber)>>(StringComparer.OrdinalIgnoreCase);

            if (searchValuesKeys == null || searchValuesKeys.Length == 0)
                return result;

            // Приводим все ключи к нижнему регистру для сравнения без учёта регистра
            var keySet = new HashSet<string>(
                searchValuesKeys.Select(k => k.Trim().ToLowerInvariant()),
                StringComparer.OrdinalIgnoreCase
            );

            foreach (var file in Directory.EnumerateFiles(iniRootDir, "*.ini", SearchOption.AllDirectories))
            {
                int lineNumber = 0;
                foreach (var line in File.ReadLines(file))
                {
                    lineNumber++;
                    var trimmed = line.Trim();

                    if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                        continue;

                    int eqIndex = trimmed.IndexOf('=');
                    if (eqIndex <= 0)
                        continue;

                    string key = trimmed.Substring(0, eqIndex).Trim().ToLowerInvariant();
                    if (!keySet.Contains(key))
                        continue;

                    string value = trimmed.Substring(eqIndex + 1).Trim();
                    if (string.IsNullOrEmpty(value))
                        continue;

                    if (!result.TryGetValue(value, out var list))
                    {
                        list = new List<(string, int)>();
                        result[value] = list;
                    }

                    list.Add((file, lineNumber));
                }
            }

            return result;
        }

        /// <summary>
        /// Считывает уникальные ключи в секции [Section] из файла filePath
        /// </summary>
        public HashSet<string> GetArmourKeysFromFile(string filePath, string nameSection = "Armour")
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            bool inArmourSection = false;

            foreach (var line in File.ReadLines(filePath))
            {
                var trimmed = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                    continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    inArmourSection = trimmed.Equals($"[{nameSection}]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }

                if (!inArmourSection)
                    continue;

                int eqIndex = trimmed.IndexOf('=');
                if (eqIndex > 0)
                {
                    var key = trimmed.Substring(0, eqIndex).Trim();
                    result.Add(key);
                }
            }

            return result;
        }

        public record FoundEntry(string FilePath, int LineNumber, string? Nickname, string KeyValue);

        /// <summary>
        /// Ищет в INI-файлах заданной директории нужные значения ключа searchKey.
        /// Возвращает список найденных записей с файлом, номером строки, nickname (если есть) и значением searchKey.
        /// </summary>
        public List<FoundEntry> FindValuesWithOptionalNickname(
            string rootDir,
            HashSet<string> targetValues,
            string searchKey = "ids_name",
            string nicknameKey = "nickname")
        {
            var result = new List<FoundEntry>();

            foreach (var file in Directory.EnumerateFiles(rootDir, "*.ini", SearchOption.AllDirectories))
            {
                try
                {
                    string? currentNickname = null;
                    int lineNumber = 0;

                    foreach (var line in File.ReadLines(file))
                    {
                        lineNumber++;

                        var trimmed = line.Trim();

                        if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                            continue;

                        if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                        {
                            // Начался новый блок, сбрасываем nickname
                            currentNickname = null;
                            continue;
                        }

                        var equalIndex = trimmed.IndexOf('=');
                        if (equalIndex <= 0)
                            continue;

                        var key = trimmed.Substring(0, equalIndex).Trim();
                        var value = trimmed.Substring(equalIndex + 1).Trim();

                        if (key.Equals(nicknameKey, StringComparison.OrdinalIgnoreCase))
                        {
                            currentNickname = value;
                        }
                        else if (key.Equals(searchKey, StringComparison.OrdinalIgnoreCase))
                        {
                            if (targetValues.Contains(value))
                            {
                                result.Add(new FoundEntry(file, lineNumber, currentNickname, value));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке файла {file}: {ex.Message}");
                }
            }

            return result;
        }
    }
}
