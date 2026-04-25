/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 25 апреля 2026 08:11:04
 * Version: 1.0.15
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
