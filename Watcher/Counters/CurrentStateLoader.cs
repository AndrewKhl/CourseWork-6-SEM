using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Watcher
{
    public class CurrentStateLoader : INotifyPropertyChanged
    {
        private double _currCPU;
        private double _currRAM;
        private double _currDisk;
        private double _currNetwork;
        private bool _loopRun = false;
        private Loader _loader;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentCPU => $"{_currCPU:F3}%";

        public string CurrentRAM => $"{_currRAM:F3}%";

        public string CurrentDisk => $"{_currDisk:F3}%";

        public string CurrentNetwork => $"{_currNetwork:F3} kbyte/s";


        public CurrentStateLoader(Loader loader)
        {
            _loader = loader;
            _loopRun = true;

            ThreadPool.QueueUserWorkItem(UpdateCounters);
        }

        public void StopLoader()
        {
            _loopRun = false;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async void UpdateCounters(object obj)
        {
            await Task.Delay(1000 - DateTime.Now.Millisecond);

            while (_loopRun)
            {
                _currCPU = _loader.GetCPULoad();
                _currRAM = Math.Max(_loader.GetRAMLoad() - 25, 0);
                _currDisk = _loader.GetDiskLoad();
                _currNetwork = _loader.GetNetworkLoad();

                OnPropertyChanged("CurrentCPU");
                OnPropertyChanged("CurrentRAM");
                OnPropertyChanged("CurrentDisk");
                OnPropertyChanged("CurrentNetwork");

                await Task.Delay(1000 - DateTime.Now.Millisecond);
            }
        }
    }
}
