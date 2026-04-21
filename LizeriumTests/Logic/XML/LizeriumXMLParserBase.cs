/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 21 апреля 2026 06:52:13
 * Version: 1.0.11
 */

namespace LizeriumTests.Logic.XML
{
    public class LizeriumXMLParserBase
    {
        protected Dictionary<string, HashSet<string>> NamesByFile { get; set; } = new();
        protected string RootDir {  get; set; }

        public LizeriumXMLParserBase(Dictionary<string, HashSet<string>> namesByFile, string rootDir)
        {
            NamesByFile = namesByFile;
            RootDir = rootDir;
        }

        public LizeriumXMLParserBase(string rootDir)
        {
            RootDir = rootDir;
        }
    }
}
