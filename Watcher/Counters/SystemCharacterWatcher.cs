﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatsonTcp;

namespace Watcher
{
    public class SystemCharacterWatcher
    {
        private double _limit;
        private int _limitDuration;
        private int _currentDuration = 0;
        private string _counterName;
        private string _pref;
        private LoggerManager _logger;
        private WatsonTcpClient _client;

        public SystemCharacterWatcher(SystemCharacterNode node, LoggerManager logger, string pref = "", WatsonTcpClient client = null)
        {
            _limit = node.Limit;
            _limitDuration = node.Duration;
            _counterName = node.Name;
            _pref = pref;

            _logger = logger;
            _client = client;
        }

        public void ExceededLimit(double value)
        {
            _currentDuration = value > _limit ? _currentDuration + 1 : 0;

            if (_currentDuration > _limitDuration)
            {
                _currentDuration = 0;
                _logger.LogMessage(_counterName, value, _limit, _pref);
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
