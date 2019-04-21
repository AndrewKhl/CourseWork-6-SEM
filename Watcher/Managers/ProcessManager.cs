using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    public class ProcessManager : INotifyPropertyChanged
    {
        private const int DurationLastCheck = 5;

        private string _processesFile;
        private int _currentCheck = 0;
        private SortedSet<string> _goodProcess;
        private LoggerManager _logger;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> GoodProcess
        {
            get
            {
                return new ObservableCollection<string>(_goodProcess);
            }
            set
            {

            }
         }

        public ProcessManager(string procFile, LoggerManager logger)
        {
            _goodProcess = new SortedSet<string>();

            _processesFile = procFile;
            _logger = logger;

            LoadGoodProcessesWithFile();
        }

        public void AddAllSystemProcess()
        {
            _goodProcess.Clear();

            foreach (Process proc in Process.GetProcesses())
                AddProcess(proc.ProcessName);

        }

        public void DeletedSelectProcess(string name)
        {
            if (_goodProcess.Contains(name))
                _goodProcess.Remove(name);
        }

        public void DeleteAllProcesses()
        {
            _goodProcess.Clear();
        }

        public void AddProcess(string name)
        {
            _goodProcess.Add(name);
        }

        private void LoadGoodProcessesWithFile()
        {
            if (File.Exists(_processesFile))
            {
                foreach (string proc in File.ReadAllLines(_processesFile, Encoding.Default))
                    AddProcess(proc);
            }
        }

        public void SaveGoodProcessesInFile()
        {
            using (var fs = new FileStream(_processesFile, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    foreach (var name in _goodProcess)
                        sw.WriteLine(name);
                }
            }

            File.SetAttributes(_processesFile, FileAttributes.Hidden);
        }

        public void CheckSystemProcess()
        {
            if (++_currentCheck == DurationLastCheck)
            {
                _currentCheck = 0;
                foreach (var proc in Process.GetProcesses())
                    if (!_goodProcess.Contains(proc.ProcessName))
                        _logger.LogBadProcess(proc.ProcessName);
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
