using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Watcher
{
    public class Loader
    {
        private readonly PerformanceCounter _cpuCounter, _ramCounter, _diskCounter;
        private List<PerformanceCounter> _networkCounters;

        private LoggerManager _logger;

        public Loader(LoggerManager logger = null)
        {
            _networkCounters = new List<PerformanceCounter>();

            _logger = logger;

            _cpuCounter = GetCounter("Processor", "% Processor Time", "_Total"); 
            _ramCounter = GetCounter("Memory", "% Committed Bytes In Use");
            _diskCounter = GetCounter("PhysicalDisk", "% Disk Time", "_Total");

            GetNetworkCounters();
        }

        public float GetCPULoad()
        {
            return _cpuCounter?.NextValue() ?? 0;
        }

        public float GetRAMLoad()
        {
            return _ramCounter?.NextValue() ?? 0;
        }

        public float GetDiskLoad()
        {
            return _diskCounter?.NextValue() ?? 0;
        }

        public float GetNetworkLoad()
        {
            float sum = 0F;

            foreach (var c in _networkCounters)
                sum += c.NextValue();

            return sum * 0.00098F; //bytes => kbytes
        }

        private void GetNetworkCounters()
        {
            var categorys = new PerformanceCounterCategory("Network Interface", System.Environment.MachineName).GetInstanceNames();

            if (categorys != null)
            {
                foreach (var cat in categorys)
                {
                    var sendCounter = GetCounter("Network Interface", "Bytes Sent/sec", cat);
                    if (sendCounter != null)
                        _networkCounters.Add(sendCounter);

                    var receivedCounter = GetCounter("Network Interface", "Bytes Received/sec", cat);
                    if (receivedCounter != null)
                        _networkCounters.Add(receivedCounter);
                }
            }
        }

        private PerformanceCounter GetCounter(string category, string counterName, string instanseName = null)
        {
            PerformanceCounter counter = null;

            try
            {
                counter = instanseName != null ? new PerformanceCounter(category, counterName, instanseName) : new PerformanceCounter(category, counterName);
            }
            catch
            {
                _logger.LogError($"Performance counter {category} {counterName} not exist in current machine");
            }

            return counter;
        }
    }
}
