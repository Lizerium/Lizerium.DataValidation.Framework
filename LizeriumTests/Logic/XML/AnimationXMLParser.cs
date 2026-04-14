/*
 * Author: Nikolay Dvurechensky and Librelancer Contributors
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 15 апреля 2026 01:09:28
 * Version: 1.0.3
 */

using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LizeriumTests.Logic.XML
{
    public class AnimationXMLParser : LizeriumXMLParserBase
    {
        private string folderCMPSXML;

        public AnimationXMLParser(Dictionary<string, HashSet<string>> namesByFile, string rootDir) : base(namesByFile, rootDir)
        {
        }

        public AnimationXMLParser(string folderCMPSXML) : base(folderCMPSXML)
        {
            this.folderCMPSXML = folderCMPSXML;
        }

        public void Parse()
        {
            foreach (var file in Directory.EnumerateFiles(RootDir, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    XDocument doc = XDocument.Load(file);
                    var animationNodes = doc.Descendants("Animation");

                    foreach (var animation in animationNodes)
                    {
                        var includeAttr = animation.Attribute("include");
                        if (includeAttr != null)
                        {
                            string includePath = includeAttr.Value.Replace("\\", Path.DirectorySeparatorChar.ToString());
                            string fullIncludePath = Path.Combine(Path.GetDirectoryName(file)!, includePath);
                            if (File.Exists(fullIncludePath))
                            {
                                ParseAnimationFile(fullIncludePath, file);
                            }
                        }
                        else
                        {
                            ParseAnimationElement(animation, file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в файле {file}: {ex.Message}");
                }
            }
        }

        private void ParseAnimationFile(string includeFilePath, string parentXmlFile)
        {
            try
            {
                var doc = XDocument.Load(includeFilePath);
                var script = doc.Descendants("Script").FirstOrDefault();
                if (script != null)
                {
                    ParseAnimationElement(script, includeFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка разбора animation include: {includeFilePath}: {ex.Message}");
            }
        }

        private void ParseAnimationElement(XElement animationElement, string sourceFile)
        {
            var scriptNode = animationElement.Element("Script") ?? animationElement;
            foreach (var scNode in scriptNode.Elements())
            {
                string? animName = scNode.Attribute("name")?.Value;
                if (!string.IsNullOrEmpty(animName))
                {
                    if (!NamesByFile.TryGetValue(animName, out var fileSet))
                    {
                        fileSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        NamesByFile[animName] = fileSet;
                    }

                    fileSet.Add(sourceFile);
                }
                else
                {
                    var name = scNode.Name.LocalName;
                    if (!NamesByFile.TryGetValue(name, out var fileSet))
                    {
                        fileSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        NamesByFile[name] = fileSet;
                    }

                    fileSet.Add(sourceFile);
                }
            }
        }

        public List<string> ParsePrisObjectCMPXML(bool existAnim = true)
        {
            var xmlFiles = Directory.GetFiles(RootDir, "*.xml", SearchOption.AllDirectories);
            var msgs = new List<string>
            {
                $"\n\t\t\t⚠ Сбор файлов в котором возможно перемещение элементов\n"
            };

            foreach (var file in xmlFiles)
            {
                try
                {
                    var doc = XDocument.Load(file);
                    var cmpnd = doc.Descendants("Cmpnd").FirstOrDefault();
                    if (cmpnd == null)
                        continue;

                    // ДО цикла по Pris — извлекаем все child'ы из Animation
                    var animationChildNames = doc.Descendants("Animation")
                        .Descendants()
                        .Where(e => e.Name.LocalName.StartsWith("Joint_map_"))
                        .Select(e => e.Element("Child_name")?.Value?.Trim())
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .ToHashSet(); // быстрое сравнение

                    var pris = cmpnd.Descendants("Pris")
                                    .Where(p => (string)p.Attribute("type") == "Rev");

                    foreach (var pr in pris)
                    {
                        var parts = pr.Elements("part");

                        foreach (var part in parts)
                        {
                            // Собираем ВСЕ комментарии в текущем <part>
                            var comments = part.Nodes().OfType<XComment>().ToList();

                            var dataPart = ParsePrisPart(part);
                           
                            if (dataPart.Count > 0 && dataPart[0] == "Root")
                            {
                                // ПРОВЕРКА: используется ли в анимации
                                if (animationChildNames.Contains(dataPart[1]))
                                    msgs.Add($"❌ Пропущен (занят в Animation): {file} -> child '{dataPart[1]}'");
                                else
                                {
                                    msgs.Add($"✅ Найден файл с вращением (Rev): {file}");
                                    msgs.Add($"   └── parent: {dataPart[0]}, child: {dataPart[1]}");
                                }
                                break; // достаточно одного совпадения
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgs.Add($"⚠ Ошибка при обработке файла {file}: {ex.Message}");
                }
            }

            msgs.Add($"\n\t\t\t⚠ Сбор файлов в котором возможно вращение элементов\n");

            foreach (var file in xmlFiles)
            {
                try
                {
                    var doc = XDocument.Load(file);
                    var cmpnd = doc.Descendants("Cmpnd").FirstOrDefault();
                    if (cmpnd == null)
                        continue;


                    // ДО цикла по Pris — извлекаем все child'ы из Animation
                    var animationChildNames = doc.Descendants("Animation")
                        .Descendants()
                        .Where(e => e.Name.LocalName.StartsWith("Joint_map_"))
                        .Select(e => e.Element("Child_name")?.Value?.Trim())
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .ToHashSet(); // быстрое сравнение

                    var pris = cmpnd.Descendants("Rev")
                                    .Where(p => (string)p.Attribute("type") == "Rev");

                    foreach (var pr in pris)
                    {
                        var parts = pr.Elements("part");

                        foreach (var part in parts)
                        {
                            // Собираем ВСЕ комментарии в текущем <part>
                            var comments = part.Nodes().OfType<XComment>().ToList();

                            string childValue = null;

                            var dataPart = ParsePrisPart(part);

                            if (dataPart.Count > 0 && dataPart[0] == "Root")
                            {
                                if (animationChildNames.Contains(dataPart[1]))
                                    msgs.Add($"❌ Пропущен (занят в Animation): {file} -> child '{dataPart[1]}'");
                                else
                                {
                                    msgs.Add($"✅ Найден файл с вращением (Rev): {file}");
                                    msgs.Add($"   └── parent: {dataPart[0]}, child: {dataPart[1]}");
                                }
                                break; // достаточно одного совпадения
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgs.Add($"⚠ Ошибка при обработке файла {file}: {ex.Message}");
                }
            }

            return msgs;
        }

        public List<string> ParsePrisPart(XElement part)
        {
            var values = new List<string>();

            // Перебираем все ноды (текст и комменты)
            foreach (var node in part.Nodes())
            {
                if (node is XText textNode)
                {
                    var parts = textNode.Value
                       .Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(s => s.Trim().Trim('\"', '\''))
                       .Where(s => s.Length > 0);

                    values.AddRange(parts);
                }
            }

            return values;
        }
    }
}
