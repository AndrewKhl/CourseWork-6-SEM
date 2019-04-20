using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Watcher
{
    internal class ConfigurationManager
    {
        private MonitoringModel _model;
        private XmlSerializer _serializer;
        private LoggerManager loggerManager;
        private string _configurationPath;

        public Dictionary<string, SystemCharacterNode> SettingsCounters { get; private set; }

        public static string CPUSectionName => "CPU";
        public static string RAMSectionName => "RAM";
        public static string NetworkSectionName => "Network";
        public static string DiskSectionName = "Disk";
        public static string ServerSectionName => "Server";

        internal ConfigurationManager(MonitoringModel model, string configPath)
        {
            SettingsCounters = new Dictionary<string, SystemCharacterNode>();

            _model = model;
            _configurationPath = configPath;
            _serializer = new XmlSerializer(typeof(List<SystemCharacterNode>));

            if (FileManager.CheckFile(configPath))
                LoadSettingsCounter();
            else
                UploadSettingsCounter();
        }

        public void UploadSettingsCounter()
        {
            SettingsCounters.Clear();

            SettingsCounters.Add(CPUSectionName, new SystemCharacterNode(_model.LoadCPU, _model.TimeLimitCPU, CPUSectionName));
            SettingsCounters.Add(RAMSectionName, new SystemCharacterNode(_model.LoadRAM, _model.TimeLimitRAM, RAMSectionName));
            SettingsCounters.Add(DiskSectionName, new SystemCharacterNode(_model.LoadDisk, _model.TimeLimitDisk, DiskSectionName));
            SettingsCounters.Add(NetworkSectionName, new SystemCharacterNode(_model.LoadNetwork, _model.TimeLimitNetwork, NetworkSectionName));

            SaveSettingCounter();
        }

        public void SaveSettingCounter()
        {
            using (var fs = new FileStream(_configurationPath, FileMode.Create))
            {
                _serializer.Serialize(fs, SettingsCounters.Values.ToList());
            }
        }

        public void LoadSettingsCounter()
        {
            try
            {
                using (var fs = new FileStream(_configurationPath, FileMode.Open))
                {
                    var counters = (List<SystemCharacterNode>)_serializer.Deserialize(fs);
                    SettingsCounters = counters.ToDictionary(c => c.Name, c => c);
                }
            }
            catch
            {
                UploadSettingsCounter();
            }

            SetSettingsCounter();
        }

        private void SetSettingsCounter()
        {
            if (SettingsCounters.ContainsKey(CPUSectionName))
            {
                _model.LoadCPU = SettingsCounters[CPUSectionName].Limit;
                _model.TimeLimitCPU = SettingsCounters[CPUSectionName].Duration;
            }

            if (SettingsCounters.ContainsKey(RAMSectionName))
            {
                _model.LoadRAM = SettingsCounters[RAMSectionName].Limit;
                _model.TimeLimitRAM = SettingsCounters[RAMSectionName].Duration;
            }

            if (SettingsCounters.ContainsKey(DiskSectionName))
            {
                _model.LoadDisk = SettingsCounters[DiskSectionName].Limit;
                _model.TimeLimitDisk = SettingsCounters[DiskSectionName].Duration;
            }

            if (SettingsCounters.ContainsKey(NetworkSectionName))
            {
                _model.LoadNetwork = SettingsCounters[NetworkSectionName].Limit;
                _model.TimeLimitNetwork = SettingsCounters[NetworkSectionName].Duration;
            }
        }
    }
}
