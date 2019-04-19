using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Watcher
{
    internal class Loader
    {
        private readonly PerformanceCounter _cpuCounter,_ramCounter, _diskCounter; 

        internal Loader()
        {
            _cpuCounter = GetCounter("Processor", "% Processor Time", "_Total"); 
            _ramCounter = GetCounter("Memory", "% Committed Bytes In Use");
            _diskCounter = GetCounter("PhysicalDisk", "% Disk Time", "_Total");
        }

        public long GetNetworkLoad()
        {
            long maxBandwidth = 0;
            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networks)
            {
                IPv4InterfaceStatistics statistics = network.GetIPv4Statistics();
                long bytesSentSpeed = statistics.BytesSent;
                long bytesReceivedSpeed = statistics.BytesReceived;

                if (bytesSentSpeed + bytesReceivedSpeed > maxBandwidth)
                {
                    maxBandwidth = bytesSentSpeed + bytesReceivedSpeed;
                }
            }
            return (long)(maxBandwidth * 0.00098);
        }

        public float GetCPULoad()
        {
            return _cpuCounter?.NextValue() ?? 0;
        }

        public  float GetRAMLoad()
        {
            return _ramCounter?.NextValue() ?? 0;
        }

        public  float GetDiskLoad()
        {
            return _diskCounter?.NextValue() ?? 0;
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

            }

            return counter;
        }
    }
}
