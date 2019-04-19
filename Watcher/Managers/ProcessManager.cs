using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    class ProcessManager
    {
        //public void AddAllSystemProcess()
        //{
        //    GoodProcess.Clear();

        //    foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
        //        GoodProcess.Add(proc.ProcessName);
        //}

        //public void DeletedSelectProcess()
        //{
        //    try
        //    {
        //        GoodProcess.RemoveAt(IndexSelectProcess);
        //    }
        //    catch { }
        //}

        //public void DeleteAllProcesses()
        //{
        //    GoodProcess.Clear();
        //}

        //public void AddProcess(string process)
        //{
        //    GoodProcess.Add(process);
        //}

        //public void LoadGoodProcessesWithFile()
        //{
        //    if (File.Exists(ProcessesFilePath))
        //    {
        //        foreach (string proc in File.ReadAllLines(ProcessesFilePath, Encoding.Default))
        //        {
        //            GoodProcess.Add(proc);
        //        }
        //    }
        //}

        //public void SaveGoodProcessesInFile()
        //{
        //    StreamWriter sw = new StreamWriter(ProcessesFilePath, true);
        //    foreach (string proc in GoodProcess)
        //        sw.WriteLine(proc);
        //    sw.Close();

        //    File.SetAttributes(ProcessesFilePath, FileAttributes.Hidden);
        //}

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
