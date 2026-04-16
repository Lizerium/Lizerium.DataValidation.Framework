/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 16 апреля 2026 11:43:45
 * Version: 1.0.6
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
