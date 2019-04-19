using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Watcher
{
    public class SystemCharacterWatcher
    {
        private double _limit;
        private int _limitDuration;
        private int _currentDuration = 0;
        private string _counterName;

        public SystemCharacterWatcher(SystemCharacterNode node)
        {
            _limit = node.Limit;
            _limitDuration = node.Duration;
            _counterName = node.Name;
        }

        public void ExceededLimit(double value)
        {
            _currentDuration = value > _limit ? _currentDuration + 1 : 0;

            if (_currentDuration > _limitDuration)
            {
                MessageBox.Show($"Exceeded limit {_counterName}");
            }
        }
    }

    [Serializable]
    public class SystemCharacterNode
    {
        public double Limit { get; set; }

        public int Duration { get; set; }

        public string Name { get; set; }


        public SystemCharacterNode() { }

        public SystemCharacterNode(double limit, int duration, string name)
        {
            Limit = limit;
            Duration = duration;
            Name = name;
        }
    }
}
