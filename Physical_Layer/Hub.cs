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
        public override event DataSent OnDataSent;

        public Hub(string name, int ports_count)
        {
            Name = name;
            Ports = new List<Port>(ports_count);
            LAN_Port p;
            for (int i = 1; i <= ports_count; i++)
            {
                p = new LAN_Port(name + "_" + i);
                p.OnCollitionDetected += new CollitionDetected(CollitionReceived);
                p.OnDataReceived += new DataReceived(ReadData);
                Ports.Add(p);
            }
        }

        private void CollitionReceived(Port port)
        {
            foreach (Port p in Ports)
            {
                if (p != port && p.Connector != null)
                {
                    p.SendCollitionMessage();
                }
            }
        }

        public override void ReadData(Data data, Port port)
        {
            if (data != null)
            {
                PhysicalL_Writer.Write_File(Clock, Name, port.Name, "receive", data.Voltage);
            }
            foreach (Port p in Ports)
            {
                if (p != port && p.Connector != null && !p.Connector.InCollition)
                {
                    p.Put_Bit_In_Port(data);
                }
            }
        }
    }
}
