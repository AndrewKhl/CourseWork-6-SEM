using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Watcher.Models;

namespace Watcher
{
    public class MonitoringModel
    {
        public double LoadCPU { get; set; } = 5;
        public double LoadRAM { get; set; } = 5;
        public double LoadNetwork { get; set; } = 100;
        public double LoadDisk { get; set; } = 5;

        public int TimeLimitCPU { get; set; } = 10;
        public int TimeLimitRAM { get; set; } = 10;
        public int TimeLimitNetwork { get; set; } = 10;
        public int TimeLimitDisk { get; set; } = 10;

        public string IpAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 9000;

        public string CurrentTheme { get; set; } = "Light";
        public string CurrentUser => $"Hello, {_currentUser.Name}";

        public bool UseGoodProcesses { get; set; } = false;
        public bool ShowMessageWindows { get; set; } = true;

        public bool CanEdited => _currentUser.IsAdmin;

        private SystemCharacterWatcher _cpuWatcher, _ramWatcher, _diskWatcher, _networkWatcher;

        private Loader _loader;
        private FileManager _fileManager;
        private ConfigurationManager _configManager;
        private LoggerManager _loggerManager;
        private ServerManager _serverManager;
        private readonly UserManager _userManager;

        private UserModel _currentUser;

        public ProcessManager ProcessManager { get; }

        private bool _runScan = false;


        public MonitoringModel(UserManager userManager, UserModel currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;

            _fileManager = new FileManager(_currentUser.Name);
            _loggerManager = new LoggerManager(_fileManager.LoggingFile);

            _loader = new Loader(_loggerManager);
            _configManager = new ConfigurationManager(this, _fileManager.ConfigurationFile, _loggerManager);
            _serverManager = new ServerManager(_loggerManager);
            ProcessManager = new ProcessManager(_fileManager.ProcessFile, _loggerManager);

            SetValuesField();
        }

        private async void Scanning(object obj)
        {
            await Task.Delay(1000 - DateTime.Now.Millisecond);

            while (_runScan)
            {
                _cpuWatcher.ExceededLimit(_loader.GetCPULoad());
                _ramWatcher.ExceededLimit(_loader.GetRAMLoad() - 25);
                _diskWatcher.ExceededLimit(_loader.GetDiskLoad());
                _networkWatcher.ExceededLimit(_loader.GetNetworkLoad());

                if (UseGoodProcesses)
                    ProcessManager.CheckSystemProcess();

                await Task.Delay(1000 - DateTime.Now.Millisecond);
            }
        }

        public void StartScanning()
        {
            _loggerManager.Start(ShowMessageWindows);

            if (IpAddress.Trim() != string.Empty)
                _serverManager.Start(IpAddress, Port);

            _configManager.UploadSettingsCounter();
            ProcessManager.RegistredUSB();

            _cpuWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.CPUSectionName], _loggerManager, "%", _serverManager);
            _ramWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.RAMSectionName], _loggerManager, "%", _serverManager);
            _diskWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.DiskSectionName], _loggerManager, "%", _serverManager);
            _networkWatcher = new SystemCharacterWatcher(_configManager.SettingsCounters[ConfigurationManager.NetworkSectionName], _loggerManager, " kbyte/sec", _serverManager);
            _runScan = true;

            ThreadPool.QueueUserWorkItem(Scanning);
        }

        public void StopSacnning()
        {
            _runScan = false;
            _configManager.UploadSettingsCounter();
            _loggerManager.Close();

            UpdateCurrentUser();
        }

        public CurrentStateLoader CreateStateLogger()
        {
            return new CurrentStateLoader(_loader);
        }

        private void UpdateCurrentUser()
        {
            _currentUser.IP = IpAddress;
            _currentUser.Port = Port;
            _currentUser.UseProcess = UseGoodProcesses;
            _currentUser.Theme = CurrentTheme;

            _userManager.UpdateUser(_currentUser);
        }

        private void SetValuesField()
        {
            if (!string.IsNullOrEmpty(_currentUser.IP))
                IpAddress = _currentUser.IP;

            Port = _currentUser.Port;
            UseGoodProcesses = _currentUser.UseProcess;

            if (_currentUser.Theme == null)
                _currentUser.Theme = CurrentTheme;
            else
                CurrentTheme = _currentUser.Theme;
        }
    }
}
