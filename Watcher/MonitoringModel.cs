using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace Watcher
{
    public class MonitoringModel
    {
        public int LoadCPU { get; set; }
        public int LoadRAM { get; set; }
        public int LoadNetwork { get; set; }
        public int LoadDisks { get; set; }

        public int TimeLimitCPU { get; set; }
        public int TimeLimitRAM { get; set; }
        public int TimeLimitNetwork { get; set; }
        public int TimeLimitDisks { get; set; }

        public static string IpAddress { get; set; } = "";
        public static int Port { get; set; }
        //public WatsonTcpClient Client { get; set; }

        public ObservableCollection<string> GoodProcess { get; set; }
        public int IndexSelectProcess { get; set; }
        public bool? UseGoodProcesses { get; set; } = false;
        private int defaultTimeToCheckProcess = 5;

        private static readonly string appdatafolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MonitorTool");
        private static readonly string LogsPath = Path.Combine(appdatafolder, "Logs.txt");
        private static readonly string ProcessesFilePath = Path.Combine(appdatafolder, "GoodProcesses.txt");
        private static readonly string ConfigPath = Path.Combine(appdatafolder, "Config.xml");

        private SortedSet<string> goodProcessesSet;

        private static readonly string LastValuesSectionName = "LastValues";
        private static readonly string CPUSectionName = "CPU";
        private static readonly string RAMSectionName = "RAM";
        private static readonly string NetworkSectionName = "Network";
        private static readonly string DiskSectionName = "Disk";
        private static readonly string ServerSectionName = "Server";

        public static readonly string CPUMessage = "Внимание! Устройство {0} не готово. Превышен лимит CPU ({1:f})";
        public static readonly string ProcessMessage = "Внимание! Устройство {0} не готово. Обнаружен незарегиcтрированный процесс ({1})";
        public static readonly string RAMMessage = "Внимание! Устройство {0} не готово. Превышен лимит RAM ({1:f})";
        public static readonly string DiskMessage = "Внимание! Устройство {0} не готово. Превышен лимит записи на диск ({1:f})";
        public static readonly string NetworkMessage = "Внимание! Устройство {0} не готово. Превышена нагрузка сети ({1:f})";

        public MonitoringModel()
        {
            if (!Directory.Exists(appdatafolder))
                Directory.CreateDirectory(appdatafolder);

            if (!File.Exists(LogsPath))
                File.Create(LogsPath);

            ReadConfig();

            GoodProcess = new ObservableCollection<string>();
            goodProcessesSet = new SortedSet<string>();

            LoadGoodProcessesWithFile();
        }

        public void AddAllSystemProcess()
        {
            GoodProcess.Clear();

            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
                GoodProcess.Add(proc.ProcessName);
        }

        public void DeletedSelectProcess()
        {
            try
            {
                GoodProcess.RemoveAt(IndexSelectProcess);
            }
            catch { }
        }

        public void DeleteAllProcesses()
        {
            GoodProcess.Clear();
        }

        public void AddProcess(string process)
        {
            GoodProcess.Add(process);
        }

        public void LoadGoodProcessesWithFile()
        {
            if (File.Exists(ProcessesFilePath))
            {
                foreach (string proc in File.ReadAllLines(ProcessesFilePath, Encoding.Default))
                {
                    GoodProcess.Add(proc);
                }
            }
        }

        public void SaveGoodProcessesInFile()
        {
            StreamWriter sw = new StreamWriter(ProcessesFilePath, true);
            foreach (string proc in GoodProcess)
                sw.WriteLine(proc);
            sw.Close();

            File.SetAttributes(ProcessesFilePath, FileAttributes.Hidden);
        }

        private string CheckSystemProcesses()
        {
            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
            {
                string res = goodProcessesSet.FirstOrDefault(p => p == proc.ProcessName);
                if (res == null)
                    return proc.ProcessName;
            }

            return null;
        }

        private void FillingGoodProcessSet()
        {
            goodProcessesSet.Clear();
            foreach (string proc in GoodProcess)
                goodProcessesSet.Add(proc);
        }

        private void WriteConfig(Dictionary<string, Tuple<Tuple<string, string>, Tuple<string, string>>> pairs)
        {
            XmlWriter writer = XmlWriter.Create(ConfigPath);
            writer.WriteStartDocument();
            writer.WriteStartElement(LastValuesSectionName);

            foreach (var pair in pairs)
            {
                writer.WriteStartElement(pair.Key);

                writer.WriteStartElement(pair.Value.Item1.Item1);
                writer.WriteString(pair.Value.Item1.Item2);
                writer.WriteEndElement();

                writer.WriteStartElement(pair.Value.Item2.Item1);
                writer.WriteString(pair.Value.Item2.Item2);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndDocument();
            writer.Close();
        }

        private void ReadConfig()
        {
            XmlDocument document = new XmlDocument();
            if (!File.Exists(ConfigPath)) return;

            try
            {
                document.Load(ConfigPath);
                LoadCPU = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{CPUSectionName}/{nameof(LoadCPU)}").InnerText);
                LoadRAM = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{RAMSectionName}/{nameof(LoadRAM)}").InnerText);
                LoadNetwork = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{NetworkSectionName}/{nameof(LoadNetwork)}").InnerText);
                LoadDisks = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{DiskSectionName}/{nameof(LoadDisks)}").InnerText);

                TimeLimitCPU = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{CPUSectionName}/{nameof(TimeLimitCPU)}").InnerText);
                TimeLimitRAM = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{RAMSectionName}/{nameof(TimeLimitRAM)}").InnerText);
                TimeLimitNetwork = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{NetworkSectionName}/{nameof(TimeLimitNetwork)}").InnerText);
                TimeLimitDisks = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{DiskSectionName}/{nameof(TimeLimitDisks)}").InnerText);

                Port = int.Parse(document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{ServerSectionName}/{nameof(Port)}").InnerText);

                string IpNode = document.DocumentElement.SelectSingleNode($"/{LastValuesSectionName}/{ServerSectionName}/{nameof(IpAddress)}").InnerText;
                if (IpNode == "" || IPAddress.TryParse(IpNode, out IPAddress empty))
                    IpAddress = IpNode;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка чтения конфига: {e.Message}");
            }
        }

        public void StartScanning()
        {
            WriteConfig(new Dictionary<string, Tuple<Tuple<string, string>, Tuple<string, string>>> {
                { CPUSectionName, Tuple.Create(Tuple.Create(nameof(LoadCPU), LoadCPU.ToString()),
                                               Tuple.Create(nameof(TimeLimitCPU), TimeLimitCPU.ToString())) },
                { RAMSectionName, Tuple.Create(Tuple.Create(nameof(LoadRAM), LoadRAM.ToString()),
                                               Tuple.Create(nameof(TimeLimitRAM), TimeLimitRAM.ToString())) },
                { NetworkSectionName, Tuple.Create(Tuple.Create(nameof(LoadNetwork), LoadNetwork.ToString()),
                                                   Tuple.Create(nameof(TimeLimitNetwork), TimeLimitNetwork.ToString())) },
                { DiskSectionName, Tuple.Create(Tuple.Create(nameof(LoadDisks), LoadDisks.ToString()),
                                                Tuple.Create(nameof(TimeLimitDisks), TimeLimitDisks.ToString())) },
                { ServerSectionName, Tuple.Create(Tuple.Create(nameof(IpAddress), IpAddress.ToString()),
                                                  Tuple.Create(nameof(Port), Port.ToString())) }
            });

            string deviceID = Environment.MachineName;
            int[] timeLimits = new int[5];

            FillingGoodProcessSet();
            goodProcessesSet.Add("backgroundTaskHost");

            //if (IpAddress != "")
            //    try
            //    {
            //        Client = new WatsonTcpClient(IpAddress, Port, ServerConnected, ServerDisconnected, MessageReceived, false);
            //    }
            //    catch (System.Net.Sockets.SocketException)
            //    {
            //        MessageBox.Show("Сервер недоступен. Сообщения будут записаны только локально");
            //    }

            int secToProcess = defaultTimeToCheckProcess;

            while (true)
            {
                #region Processes
                if (UseGoodProcesses == true)
                {
                    string processResult = CheckSystemProcesses();
                    if (processResult != null)
                    {
                        if (secToProcess == defaultTimeToCheckProcess)
                        {
                         //   LogMessage(ProcessMessage, deviceID, processResult, Client);
                            secToProcess = 0;
                        }
                        else
                            secToProcess++;
                    }
                }
                #endregion

                #region CPU
                double currentCPULoad = Monitor.GetCPULoad();
                if (LoadCPU < currentCPULoad)
                    timeLimits[0]++;
                else
                    timeLimits[0] = 0;

                if (timeLimits[0] > TimeLimitCPU)
                {
                    //LogMessage(CPUMessage, deviceID, currentCPULoad.ToString(), Client);
                    timeLimits[0] = 0;
                }
                #endregion

                #region RAM
                double currentRAMLoad = Monitor.GetRAMLoad();
                if (LoadRAM < currentRAMLoad)
                    timeLimits[1]++;
                else
                    timeLimits[1] = 0;

                if (timeLimits[1] > TimeLimitRAM)
                {
                    //LogMessage(RAMMessage, deviceID, currentRAMLoad.ToString(), Client);
                    timeLimits[1] = 0;
                }
                #endregion

                #region Disk
                double currentDiskLoad = Monitor.GetDiskLoad();
                if (LoadDisks < currentDiskLoad)
                    timeLimits[2]++;
                else
                    timeLimits[2] = 0;

                if (timeLimits[2] > TimeLimitDisks)
                {
                    //LogMessage(DiskMessage, deviceID, currentDiskLoad.ToString(), Client);
                    timeLimits[2] = 0;
                }
                #endregion

                #region Network
                long currentNetworkLoad = Monitor.GetNetworkLoad();
                if (LoadNetwork < currentNetworkLoad)
                    timeLimits[3]++;
                else
                    timeLimits[3] = 0;

                if (timeLimits[3] > TimeLimitNetwork)
                {
                    //LogMessage(NetworkMessage, deviceID, currentNetworkLoad.ToString(), Client);
                    timeLimits[3] = 0;
                }
                #endregion

                Thread.Sleep(1000);
            }
        }

        //static void LogMessage(string messageFormat, string deviceID, string parameter, WatsonTcpClient client)
        //{
        //    string message = string.Format(messageFormat, deviceID, parameter);
        //    LogMessageInFile(message);
        //    if (client != null && IpAddress != null && IpAddress != "")
        //        client.Send(Encoding.UTF8.GetBytes(message));
        //    MessageBox.Show(message);
        //}

        //private static void LogMessageInFile(string message)
        //{
        //    string date = DateTime.Now.ToString("dd-MMMM-yyyy HH:mm:ss ");
        //    StreamWriter logger = new StreamWriter(LogsPath, true, Encoding.UTF8);
        //    logger.WriteLine(date + message);
        //    logger.Close();
        //}

        //static bool MessageReceived(byte[] data)
        //{
        //    MessageBox.Show("Сообщение от сервера: " + Encoding.UTF8.GetString(data));
        //    return true;
        //}

        //static bool ServerConnected()
        //{
        //    MessageBox.Show("Сервер подключен");
        //    return true;
        //}

        //static bool ServerDisconnected()
        //{
        //    MessageBox.Show("Сервер отключен");
        //    return true;
        //}
    }
}
