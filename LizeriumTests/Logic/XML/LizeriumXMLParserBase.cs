/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 24 июня 2026 10:34:39
 * Version: 1.0.76
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
