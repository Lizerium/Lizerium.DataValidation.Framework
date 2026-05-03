/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 03 мая 2026 06:52:22
 * Version: 1.0.24
 */

using System.Diagnostics;

using LizeriumTests.Components;

using Newtonsoft.Json;

namespace LizeriumTests.Logic
{
    public class SettingsService
    {
        public string SETTINGS_PATH => Path.Combine(AppContext.BaseDirectory, "SETTINGS");
        public string SETTINGS_FILE => Path.Combine(SETTINGS_PATH, "app_settings.json");

        public bool IsCorrectSettings { get; set; } = true;
        public List<string> SettingsErrors = new List<string>();

        public SettingsTestsData SettingsTestsData { get; set; }

        public bool IsCorrectINI { get; set; } = true;
        public bool IsCorrectCMP { get; set; } = true;
        public bool IsCorrectCMPXML { get; set; } = true;
        public bool IsCorrect3DB { get; set; } = true;
        public bool IsCorrectDLL { get; set; } = true;
        public bool IsCorrectMAT { get; set; } = true;
        public bool IsCorrectWAV { get; set; } = true;
        public bool IsCorrectTXM { get; set; } = true;
        public bool IsCorrectSPH { get; set; } = true;

        public SettingsService() 
        {
            if(!Directory.Exists(SETTINGS_PATH))
                Directory.CreateDirectory(SETTINGS_PATH);
            if(!File.Exists(SETTINGS_FILE))
            {
                var settings = new SettingsTestsData()
                {
                    Folder3DBS = "path/to/folder3dbs",
                    FolderCMPS = "path/to/foldercmps",
                    FolderCMPSXML = "path/to/foldercmpsxml",
                    FolderDLLS = "path/to/folderdlls",
                    FolderINIS = "path/to/folderinis", 
                    FolderMATS = "path/to/foldermats",
                    FolderSPHS = "path/to/foldersphs",
                    FolderWAVS = "path/to/folderwavs",
                    FolderTXMS = "path/to/foldertxms"
                };
                SettingsTestsData = settings;
                // Сериализация объекта в JSON
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                // Запись в файл
                File.WriteAllText(SETTINGS_FILE, json);
                // открываем файл для заполнения
                Process.Start(SETTINGS_PATH);
                IsCorrectSettings = false;
                SettingsErrors.Add($"Error Settings file not found. Autogenerate! Please write file!\n");
                return;
            }

            var data = File.ReadAllText(SETTINGS_FILE);
            var settingsGet = JsonConvert.DeserializeObject<SettingsTestsData>(data);
            SettingsTestsData = settingsGet;
            if (settingsGet == null)
            {
                SettingsErrors.Add($"Error Settings not found to file {SETTINGS_FILE}\n");
            }
            if (!Directory.Exists(settingsGet?.Folder3DBS))
            {
                IsCorrect3DB = false;
                SettingsErrors.Add("Error Folder 3DBS\n");
            }
            if (!Directory.Exists(settingsGet?.FolderCMPS))
            {
                IsCorrectCMP = false;
                SettingsErrors.Add("Error Folder CMPS\n");
            }
            if (!Directory.Exists(settingsGet?.FolderCMPSXML))
            {
                IsCorrectCMPXML = false;
                SettingsErrors.Add("Error Folder CMPS XML\n");
            }
            if (!Directory.Exists(settingsGet?.FolderDLLS))
            {
                IsCorrectDLL = false;
                SettingsErrors.Add("Error Folder DLLS\n");
            }
            if (!Directory.Exists(settingsGet?.FolderSPHS))
            {
                IsCorrectSPH = false;
                SettingsErrors.Add("Error Folder SPHS\n");
            }
            if (!Directory.Exists(settingsGet?.FolderINIS))
            {
                IsCorrectINI = false;
                SettingsErrors.Add("Error Folder INIS\n");
            }
            if (!Directory.Exists(settingsGet?.FolderMATS))
            {
                IsCorrectMAT = false;
                SettingsErrors.Add("Error Folder MATS\n");
            }
            if (!Directory.Exists(settingsGet?.FolderWAVS))
            {
                IsCorrectWAV = false;
                SettingsErrors.Add("Error Folder WAVS\n");
            }
            if (!Directory.Exists(settingsGet?.FolderTXMS))
            {
                IsCorrectTXM = false;
                SettingsErrors.Add("Error Folder TXMS\n");
            }
        }
    }
}
