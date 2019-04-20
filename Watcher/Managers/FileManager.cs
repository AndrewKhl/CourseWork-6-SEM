using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    internal class FileManager
    {
        private static readonly string _appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MonitorTool");

        public string ConfigurationFile => Path.Combine(_appDataFolder, "Config.xml");

        public string LoggingFile => Path.Combine(_appDataFolder, "Log.txt");

        internal FileManager()
        {
            CreateFolder(_appDataFolder);
        }

        public static void CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public static bool CheckFile(string path)
        {
            return File.Exists(path);
        }
        //private static readonly string LogsPath = Path.Combine(appdatafolder, "Logs.txt");
        //private static readonly string ProcessesFilePath = Path.Combine(appdatafolder, "GoodProcesses.txt");
        //private static readonly string ConfigPath = Path.Combine(appdatafolder, "Config.xml");
    }
}
