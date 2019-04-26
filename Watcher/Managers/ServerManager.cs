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
                _client = new WatsonTcpClient(ip, port)
                {
                    ServerConnected = SuccessfullyСonnected,
                    Debug = false
                };
                _client.Start();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.ServerNotAvaible, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.LogError("Server is not available");
            }
        }

        private bool SuccessfullyСonnected()
        {
            MessageBox.Show(Properties.Resources.ServerConnected, "", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        public void SendMessage(string message)
        {
            _client?.SendAsync(Encoding.UTF8.GetBytes(message));
        }
    }
}
