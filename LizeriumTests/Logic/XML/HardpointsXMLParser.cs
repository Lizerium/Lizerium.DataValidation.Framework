/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 16 апреля 2026 11:43:45
 * Version: 1.0.6
 */

using System.Xml.Linq;

namespace LizeriumTests.Logic.XML
{
    public class HardpointsXMLParser : LizeriumXMLParserBase
    {
        public HardpointsXMLParser(Dictionary<string, HashSet<string>> namesByFile, string rootDir) : base(namesByFile, rootDir)
        {
        }

        public void Parse()
        {
            foreach (var file in Directory.EnumerateFiles(RootDir, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    XDocument doc = XDocument.Load(file);
                    var utfRoot = doc.Element("UTFXML")?.Element("UTF_ROOT");
                    if (utfRoot == null)
                        continue;

                    foreach (var node in utfRoot.Elements())
                    {
                        ParseNode(file, node);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения {file}: {ex.Message}");
                }
            }
        }

        private void ParseNode(string filePath, XElement node)
        {
            foreach (var child in node.Elements())
            {
                if (child.Name.LocalName == "Hardpoints")
                {
                    var hardpointNames = ExtractHardpointNames(child);

                    if (!NamesByFile.TryGetValue(filePath, out var set))
                        NamesByFile[filePath] = set = new HashSet<string>();

                    foreach (var name in hardpointNames)
                        set.Add(name);
                }

                if (child.HasElements)
                    ParseNode(filePath, child);
            }
        }

        private List<string> ExtractHardpointNames(XElement hardpointsElement)
        {
            var names = new List<string>();
            foreach (var group in hardpointsElement.Elements()) // Fixed, Revolute, etc.
            {
                foreach (var hp in group.Elements())
                {
                    names.Add(hp.Name.LocalName); // e.g., "HpMount", "DpCntrltwr01"
                }
            }
            return names;
        }
    }
}
