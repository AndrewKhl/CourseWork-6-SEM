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

        static string serverIp = "";
        static int serverPort = 0;
        static WatsonTcpServer server = null;

        static void Main(string[] args)
        {
            if (!Directory.Exists(_appDataFolder))
                Directory.CreateDirectory(_appDataFolder);

            serverIp = Common.InputString("Server IP:", "127.0.0.1", false);
            serverPort = Common.InputInteger("Server port:", 9000, true, false);
            server = new WatsonTcpServer(serverIp, serverPort); 

            server.ClientConnected = ClientConnected;
            server.ClientDisconnected = ClientDisconnected;
            server.MessageReceived = MessageReceived;

            server.Start();

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
                        clients = server.ListClients();
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
                        server.DisconnectClient(ipPort);
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
            {
                msg = Encoding.UTF8.GetString(data);
            }

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
