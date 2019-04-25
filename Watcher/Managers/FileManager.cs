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

        public string ConfigurationFile => Path.Combine(_workingFolder, "Config.xml");

        public string LoggingFile => Path.Combine(_workingFolder, "Log.txt");

        public string ProcessFile => Path.Combine(_workingFolder, "Process.txt");

        private string _workingFolder;

        internal FileManager(string currentUser = "")
        {
            _workingFolder = currentUser == "" ? _appDataFolder : Path.Combine(_appDataFolder, currentUser);

            CreateFolder(_workingFolder);
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
    }
}
