using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{
    public class Host : Device
    {
        private Port p;
        public Host(string name)
        {
            Name = name;
            p = new LAN_Port(name + "_1");
            Ports = new List<Port>(1);
            p.OnDataReceived += new DataReceived(ReadData);
            Ports.Add(p);
        }

        //public override event BitSent OnBitSent;
        //public override event DataSent OnDataSent;

        public override void ReadData(Data data, Port p)
        {
            PhysicalL_Writer.Write_File(1, Name, Ports[0].Name, "receive", data.Voltage, false);
        }

        public override void SendData(Data data, int signal_time)
        {
            PhysicalL_Writer.Write_File(12, Name, p.Name, "send", data.Voltage, false);
            p.Put_Bit_In_Port(data);
            //OnBitSent(data);
        }
    }
}
