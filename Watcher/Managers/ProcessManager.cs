using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    class ProcessManager
    {
        private string _processesFile;
        private SortedSet<string> _goodProcess;
        private LoggerManager _logger;

        public ProcessManager(string procFile, LoggerManager logger)
        {
            _goodProcess = new SortedSet<string>();

            _processesFile = procFile;
            _logger = logger;
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
            foreach
        }

        //private string CheckSystemProcesses()
        //{
        //    foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
        //    {
        //        string res = goodProcessesSet.FirstOrDefault(p => p == proc.ProcessName);
        //        if (res == null)
        //            return proc.ProcessName;
        //    }

        //    return null;
        //}

        //private void FillingGoodProcessSet()
        //{
        //    goodProcessesSet.Clear();
        //    foreach (string proc in GoodProcess)
        //        goodProcessesSet.Add(proc);
        //}

        //    //goodProcessesSet.Add("backgroundTaskHost");

        //    int secToProcess = defaultTimeToCheckProcess;

        //if (UseGoodProcesses == true)
        //        //{
        //        //    string processResult = CheckSystemProcesses();
        //        //    if (processResult != null)
        //        //    {
        //        //        if (secToProcess == defaultTimeToCheckProcess)
        //        //        {
        //        //         //   LogMessage(ProcessMessage, deviceID, processResult, Client);
        //        //            secToProcess = 0;
        //        //        }
        //        //        else
        //        //            secToProcess++;
        //        //    }
        //        //}
    }
}
