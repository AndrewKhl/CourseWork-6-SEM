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
        public int LoadDisk { get; set; }

        public int TimeLimitCPU { get; set; }
        public int TimeLimitRAM { get; set; }
        public int TimeLimitNetwork { get; set; }
        public int TimeLimitDisk { get; set; }

        public static string IpAddress { get; set; } = "";
        public static int Port { get; set; }
        //public WatsonTcpClient Client { get; set; }

        public ObservableCollection<string> GoodProcess { get; set; }
        public int IndexSelectProcess { get; set; }
        public bool? UseGoodProcesses { get; set; } = false;
        private int defaultTimeToCheckProcess = 5;

        private SortedSet<string> goodProcessesSet;
        SystemCharacterWatcher _cpuWatcher, _ramWatcher, _diskWatcher;

        //private static readonly string LastValuesSectionName = "LastValues";
        private static readonly string CPUSectionName = "CPU";
        private static readonly string RAMSectionName = "RAM";
        private static readonly string NetworkSectionName = "Network";
        private static readonly string DiskSectionName = "Disk";
        private static readonly string ServerSectionName = "Server";

        //public static readonly string CPUMessage = "Внимание! Устройство {0} не готово. Превышен лимит CPU ({1:f})";
        //public static readonly string ProcessMessage = "Внимание! Устройство {0} не готово. Обнаружен незарегиcтрированный процесс ({1})";
        //public static readonly string RAMMessage = "Внимание! Устройство {0} не готово. Превышен лимит RAM ({1:f})";
        //public static readonly string DiskMessage = "Внимание! Устройство {0} не готово. Превышен лимит записи на диск ({1:f})";
        //public static readonly string NetworkMessage = "Внимание! Устройство {0} не готово. Превышена нагрузка сети ({1:f})";

        private Loader _loader;
        private bool _runScan = false;


        public MonitoringModel()
        {
            //if (!Directory.Exists(appdatafolder))
            //    Directory.CreateDirectory(appdatafolder);

            //if (!File.Exists(LogsPath))
            //    File.Create(LogsPath);

            //ReadConfig();

            _loader = new Loader();

            //GoodProcess = new ObservableCollection<string>();
            //goodProcessesSet = new SortedSet<string>();

            //LoadGoodProcessesWithFile();
        }

        private async void Scanning(object obj)
        {
            await Task.Delay(1000 - DateTime.Now.Millisecond);

            while (_runScan)
            {
                _cpuWatcher.ExceededLimit(_loader.GetCPULoad());
                _ramWatcher.ExceededLimit(_loader.GetRAMLoad());
                _diskWatcher.ExceededLimit(_loader.GetDiskLoad());

                await Task.Delay(1000 - DateTime.Now.Millisecond);
            }
        }

        public void StartScanning()
        {
            _cpuWatcher = new SystemCharacterWatcher(LoadCPU, TimeLimitCPU, CPUSectionName);
            _ramWatcher = new SystemCharacterWatcher(LoadRAM, TimeLimitRAM, RAMSectionName);
            _diskWatcher = new SystemCharacterWatcher(LoadDisk, TimeLimitDisk, DiskSectionName);
            _runScan = true;

            ThreadPool.QueueUserWorkItem(Scanning);
        }

        public void StopSacnning()
        {
            _runScan = false;
        }
    }
}
