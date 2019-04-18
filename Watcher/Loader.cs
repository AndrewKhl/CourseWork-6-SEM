using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Watcher
{
    class Loader
    {
        private  PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private  PerformanceCounter ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
        private  PerformanceCounter diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

        public  long GetNetworkLoad()
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

        public  float GetCPULoad()
        {
            return cpuCounter.NextValue();
        }

        public  float GetRAMLoad()
        {
            return ramCounter.NextValue();
        }

        public  float GetDiskLoad()
        {
            return diskCounter.NextValue();
        }
    }
}
