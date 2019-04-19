using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    class ConfigurationManager
    {
        //private void WriteConfig(Dictionary<string, Tuple<Tuple<string, string>, Tuple<string, string>>> pairs)
        //{
        //    XmlWriter writer = XmlWriter.Create(ConfigPath);
        //    writer.WriteStartDocument();
        //    writer.WriteStartElement(LastValuesSectionName);

        //    foreach (var pair in pairs)
        //    {
        //        writer.WriteStartElement(pair.Key);

        //        writer.WriteStartElement(pair.Value.Item1.Item1);
        //        writer.WriteString(pair.Value.Item1.Item2);
        //        writer.WriteEndElement();

        //        writer.WriteStartElement(pair.Value.Item2.Item1);
        //        writer.WriteString(pair.Value.Item2.Item2);
        //        writer.WriteEndElement();

        //        writer.WriteEndElement();
        //    }

        //    writer.WriteEndDocument();
        //    writer.Close();
        //}

        //public void StartScanning()
        //{
        //    //WriteConfig(new Dictionary<string, Tuple<Tuple<string, string>, Tuple<string, string>>> {
        //    //    { CPUSectionName, Tuple.Create(Tuple.Create(nameof(LoadCPU), LoadCPU.ToString()),
        //    //                                   Tuple.Create(nameof(TimeLimitCPU), TimeLimitCPU.ToString())) },
        //    //    { RAMSectionName, Tuple.Create(Tuple.Create(nameof(LoadRAM), LoadRAM.ToString()),
        //    //                                   Tuple.Create(nameof(TimeLimitRAM), TimeLimitRAM.ToString())) },
        //    //    { NetworkSectionName, Tuple.Create(Tuple.Create(nameof(LoadNetwork), LoadNetwork.ToString()),
        //    //                                       Tuple.Create(nameof(TimeLimitNetwork), TimeLimitNetwork.ToString())) },
        //    //    { DiskSectionName, Tuple.Create(Tuple.Create(nameof(LoadDisks), LoadDisks.ToString()),
        //    //                                    Tuple.Create(nameof(TimeLimitDisks), TimeLimitDisks.ToString())) },
        //    //    { ServerSectionName, Tuple.Create(Tuple.Create(nameof(IpAddress), IpAddress.ToString()),
        //    //                                      Tuple.Create(nameof(Port), Port.ToString())) }
        //    //});
    }
}
