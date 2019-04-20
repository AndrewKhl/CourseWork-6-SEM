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
        public double LoadCPU { get; set; }
        public double LoadRAM { get; set; }
        public double LoadNetwork { get; set; }
        public double LoadDisk { get; set; }

        public int TimeLimitCPU { get; set; }
        public int TimeLimitRAM { get; set; }
        public int TimeLimitNetwork { get; set; }
        public int TimeLimitDisk { get; set; }

        public string IpAddress { get; set; } = "";
        public int Port { get; set; }

        public ObservableCollection<string> GoodProcess { get; set; }
        public int IndexSelectProcess { get; set; }
        public bool UseGoodProcesses { get; set; } = false;

        private SortedSet<string> goodProcessesSet;
        private SystemCharacterWatcher _cpuWatcher, _ramWatcher, _diskWatcher, _networkWatcher;

        private Loader _loader;
        private FileManager _fileManager;
        private ConfigurationManager _configManager;
        private LoggerManager _loggerManager;

        private bool _runScan = false;


        public MonitoringModel()
        {
            _fileManager = new FileManager();
            _loggerManager = new LoggerManager(_fileManager.LoggingFile);

            _loader = new Loader(_loggerManager);
            _configManager = new ConfigurationManager(this, _fileManager.ConfigurationFile, _loggerManager);
        }

        private async void Scanning(object obj)
        {
            await Task.Delay(1000 - DateTime.Now.Millisecond);

            while (_runScan)
            {
                _cpuWatcher.ExceededLimit(_loader.GetCPULoad());
                _ramWatcher.ExceededLimit(_loader.GetRAMLoad());
                _diskWatcher.ExceededLimit(_loader.GetDiskLoad());
                _networkWatcher.ExceededLimit(_loader.GetNetworkLoad());

                await Task.Delay(1000 - DateTime.Now.Millisecond);
            }
        }

        public void StartScanning()
        {
            _configManager.UploadSettingsCounter();

            _cpuWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.CPUSectionName], _loggerManager);
            _ramWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.RAMSectionName], _loggerManager);
            _diskWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.DiskSectionName], _loggerManager);
            _networkWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.NetworkSectionName], _loggerManager);
            _runScan = true;

            ThreadPool.QueueUserWorkItem(Scanning);
        }

        public void StopSacnning()
        {
            _runScan = false;
        }
    }
}
