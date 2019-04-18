using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    public class SystemCharacterWatcher
    {
        private double _limit;
        private int _limitDuration;
        private int _currentDuration = 0;
        private string _counterName;

        public SystemCharacterWatcher(int limit, int duration, string counter)
        {
            _limit = limit;
            _limitDuration = duration;
            _counterName = counter;
        }

        public void ExceededLimit(double value)
        {
            _currentDuration = value > _limit ? _currentDuration + 1 : 0;

            if (_currentDuration > _limitDuration)
            {

            }
        }
    }
}
