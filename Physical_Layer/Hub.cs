using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{


    public class Hub : Device
    {
        private Port receiving_port;
        public Hub(string name, int ports_count)
        {
            Name = name;
            Ports = new List<Port>(ports_count);
            LAN_Port p;
            for (int i = 1; i <= ports_count; i++)
            {
                p = new LAN_Port(name + "_" + i);
                p.OnDataReceived += new DataReceived(ReadData);
                Ports.Add(p);
            }
        }

        public override void ReadData(Data data, Port p)
        {
            receiving_port = p;
            PhysicalL_Writer.Write_File(1, Name, receiving_port.Name, "receive", receiving_port.DataInPort.Voltage, false);
            SendData(data, 1);
        }

        public override void SendData(Data data, int signal_time)
        {
            foreach (Port p in Ports)
            {
                if (!p.Equals(receiving_port) && p.Connector != null)
                {
                    PhysicalL_Writer.Write_File(12, Name, p.Name, "send", data.Voltage, false);
                    p.Put_Bit_In_Port(data);
                }
            }
        }
    }
}
