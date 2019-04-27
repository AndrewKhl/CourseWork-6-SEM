using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;

namespace Watcher
{
    public class ProcessManager
    {
        private const int DurationLastCheck = 5;

        private string _processesFile;
        private int _currentCheck = 0;
        private LoggerManager _logger;

        private SortedSet<string> _acceptUSB;

        public ObservableCollection<string> GoodProcess { get; }


        public ProcessManager(string procFile, LoggerManager logger)
        {
            GoodProcess = new ObservableCollection<string>();
            _acceptUSB = new SortedSet<string>();

            _processesFile = procFile;
            _logger = logger;

            LoadGoodProcessesWithFile();
        }

        public void AddAllSystemProcess()
        {
            GoodProcess.Clear();

            foreach (Process proc in Process.GetProcesses())
                AddProcess(proc.ProcessName);
        }

        public void DeletedSelectProcess(string name)
        {
            if (GoodProcess.Contains(name))
                GoodProcess.Remove(name);
        }

        public void DeleteAllProcesses()
        {
            GoodProcess.Clear();
        }

        public bool AddProcess(string name)
        {
            if (!GoodProcess.Contains(name))
            {
                GoodProcess.Add(name);
                return true;
            }
            return false;
        }

        public void SaveGoodProcessesInFile()
        {
            using (var fs = new FileStream(_processesFile, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    foreach (var name in GoodProcess)
                        sw.WriteLine(name);
                }
            }
        }

        public void CheckSystemProcess(ServerManager client)
        {
            if (++_currentCheck == DurationLastCheck)
            {
                _currentCheck = 0;
                foreach (var proc in Process.GetProcesses())
                    if (!GoodProcess.Contains(proc.ProcessName) && proc.ProcessName != "backgroundTaskHost")
                        _logger.LogBadProcess(proc.ProcessName, client);

                CheckUSB(client);
            }
        }

        public void RegistredUSB()
        {
            _acceptUSB.Clear();

            using (var mbs = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'"))
            {
                foreach (ManagementObject mo in mbs.Get())
                {
                    var name = mo["PNPDeviceID"].ToString();
                    _acceptUSB.Add(name.Substring(name.LastIndexOf('\\') + 1));
                }
            }
        }

        private void CheckUSB(ServerManager client)
        {
            using (var mbs = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'"))
            {
                foreach (ManagementObject mo in mbs.Get())
                {
                    var name = mo["PNPDeviceID"].ToString();
                    name = name.Substring(name.LastIndexOf('\\') + 1);

                    if (!_acceptUSB.Contains(name))
                        _logger.LogUnregistredUSB(name, client);
                }
            }
        }

        private void LoadGoodProcessesWithFile()
        {
            if (File.Exists(_processesFile))
            {
                foreach (string proc in File.ReadAllLines(_processesFile, Encoding.Default))
                    AddProcess(proc);
            }
        }
    }
}
