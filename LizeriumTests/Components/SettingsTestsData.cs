/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 22 апреля 2026 18:37:28
 * Version: 1.0.12
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
