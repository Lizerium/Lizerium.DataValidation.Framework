/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 01 июля 2026 08:35:58
 * Version: 1.0.83
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizeriumTests.Components.Collections
{
    public class ResultSearchCollection
    {
        public Dictionary<string, List<ResultSearchItem>> Result { get; set; } = new Dictionary<string, List<ResultSearchItem>>(StringComparer.OrdinalIgnoreCase);
    }

    public class ResultSearchItem
    {
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
        public string OtherKeyValue { get; set; }

        public ResultSearchItem(string filePath,
                                int lineNumber, 
                                string otherKeyValue = "")
        {
            FilePath = filePath;
            LineNumber = lineNumber;
            OtherKeyValue = otherKeyValue;
        }
    }
}
