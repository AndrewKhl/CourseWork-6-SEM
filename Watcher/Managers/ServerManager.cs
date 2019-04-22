using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatsonTcp;

namespace Watcher
{
    public class ServerManager
    {
        private WatsonTcpClient _client;
        private LoggerManager _logger;

        public ServerManager(LoggerManager logger)
        {
            _logger = logger;
        }

        public void Start(string ip, int port)
        {
            try
            {
                _client = new WatsonTcpClient(ip, port);
                _client.ServerConnected = SuccessfullyСonnected;
                _client.Debug = false;
                _client.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server is not available", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.LogError(ex.Message);
            }
        }

        private bool SuccessfullyСonnected()
        {
            MessageBox.Show("Server connected", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        public void SendMessage(string message)
        {
            _client?.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
