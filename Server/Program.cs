using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WatsonTcp;

namespace Server
{
    class Program
    {
        private static readonly string _appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MonitorTool", "Server");

        private static string _serverIp = "";
        private static int _serverPort = 0;
        private static WatsonTcpServer _server = null;


        static void Main(string[] args)
        {
            if (!Directory.Exists(_appDataFolder))
                Directory.CreateDirectory(_appDataFolder);

            _serverIp = Common.InputString("Server IP:", "127.0.0.1", false);
            _serverPort = Common.InputInteger("Server port:", 9000, true, false);
            _server = new WatsonTcpServer(_serverIp, _serverPort); 

            _server.ClientConnected = ClientConnected;
            _server.ClientDisconnected = ClientDisconnected;
            _server.MessageReceived = MessageReceived;

            _server.Start();

            LogMessage("Server start");

            bool runForever = true;
            while (runForever)
            {
                Console.Write("Command [? for help]: ");
                string userInput = Console.ReadLine();

                List<string> clients;
                string ipPort;

                if (String.IsNullOrEmpty(userInput))
                {
                    continue;
                }

                switch (userInput)
                {
                    case "?":
                        Console.WriteLine("Available commands:");
                        Console.WriteLine("  ?        help (this menu)");
                        Console.WriteLine("  q        quit");
                        Console.WriteLine("  cls      clear screen");
                        Console.WriteLine("  list     list clients");
                        Console.WriteLine("  send     send message to client");
                        Console.WriteLine("  remove   disconnect client");
                        break;

                    case "q":
                        runForever = false;
                        break;

                    case "cls":
                        Console.Clear();
                        break;

                    case "list":
                        clients = _server.ListClients();
                        if (clients != null && clients.Count > 0)
                        {
                            Console.WriteLine("Clients");
                            foreach (string curr in clients)
                            {
                                Console.WriteLine("  " + curr);
                            }
                        }
                        else
                        {
                            Console.WriteLine("None");
                        }
                        break;

                    case "remove":
                        Console.Write("IP:Port: ");
                        ipPort = Console.ReadLine();
                        _server.DisconnectClient(ipPort);
                        break;

                    default:
                        break;
                }
            }
        }

        static bool ClientConnected(string ipPort)
        {
            Console.WriteLine("Client connected: " + ipPort);
            return true;
        }

        static bool ClientDisconnected(string ipPort)
        {
            Console.WriteLine("Client disconnected: " + ipPort);
            return true;
        }

        static bool MessageReceived(string ipPort, byte[] data)
        {
            string msg = "";

            if (data != null && data.Length > 0)
                msg = Encoding.UTF8.GetString(data);
            else
                return true;

            Console.WriteLine("Message received from " + ipPort + ": " + msg);
            LogMessage($"{ipPort} {msg}");
            return true;
        }

        static void LogMessage(string message)
        {
            using (var fs = new FileStream(Path.Combine(_appDataFolder, "ServerLog.txt"), FileMode.Append))
            {
                using (var sw = new StreamWriter(fs))
                    sw.WriteLine($"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} {message}");
            }
        }
    }
}
