using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    class LoggerManager
    {
        //static void LogMessage(string messageFormat, string deviceID, string parameter, WatsonTcpClient client)
        //{
        //    string message = string.Format(messageFormat, deviceID, parameter);
        //    LogMessageInFile(message);
        //    if (client != null && IpAddress != null && IpAddress != "")
        //        client.Send(Encoding.UTF8.GetBytes(message));
        //    MessageBox.Show(message);
        //}

        //private static void LogMessageInFile(string message)
        //{
        //    string date = DateTime.Now.ToString("dd-MMMM-yyyy HH:mm:ss ");
        //    StreamWriter logger = new StreamWriter(LogsPath, true, Encoding.UTF8);
        //    logger.WriteLine(date + message);
        //    logger.Close();
        //}
    }
}
