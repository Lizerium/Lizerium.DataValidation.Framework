/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 июня 2026 11:42:52
 * Version: 1.0.80
 */

using Newtonsoft.Json;

namespace LizeriumTests.Components
{
    public class SettingsTestsData
    {
        [JsonProperty("ini")]
        public string FolderINIS { get; set; }
        [JsonProperty("music")]
        public string FolderWAVS { get; set; }
        [JsonProperty("cmp")]
        public string FolderCMPS { get; set; }
        [JsonProperty("cmpxml")]
        public string FolderCMPSXML { get; set; }
        [JsonProperty("3db")]
        public string Folder3DBS { get; set; }
        [JsonProperty("dll")]
        public string FolderDLLS { get; set; }
        [JsonProperty("txm")]
        public string FolderTXMS { get; set; }
        [JsonProperty("mat")]
        public string FolderMATS { get; set; }
        [JsonProperty("sph")]
        public string FolderSPHS { get; set; }
    }
}
