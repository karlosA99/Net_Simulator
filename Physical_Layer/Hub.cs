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
        public override event BitSent OnBitSent;
        public override event DataSent OnDataSent;

        public Hub(string name, int ports_count)
        {
            Name = name;
            Ports = new List<Port>(ports_count);
            LAN_Port p;
            for (int i = 1; i <= ports_count; i++)
            {
                p = new LAN_Port(name + "_" + i);
                Ports.Add(p);
            }
        }

        public override void ReadData(int time)
        {
            LAN_Port receiving_port = null;

            foreach (LAN_Port item in Ports)
            {
                if (item.Connector != null && ((Simple_Wire)item.Connector).BitOnWire != null)
                {
                    receiving_port = item;
                }
            }
            receiving_port.Put_Bit_In_Port(((Simple_Wire)receiving_port.Connector).BitOnWire);
            PhysicalL_Writer.Write_File(time, Name, receiving_port.Name, "receive", (int)receiving_port.DataInPort.Voltage, false);
            
        }

        public override void SendData(Data data, int signal_time)
        {
            int i_time = signal_time;//este int es un clone de signal_time que se puede modificar sin afectar a signal_time
            while (i_time > 0)
            {
                foreach (Port p in Ports)
                {
                    if (p.Connector != null && ((Wire)p.Connector).BitOnWire == null)
                    {
                        IConnector aux = p.Connector;
                        ((Wire)aux).BitOnWire = data;
                        PhysicalL_Writer.Write_File(12, Name, p.Name, "send", data.Voltage, false);
                    }
                }
                i_time--;
            }
            OnBitSent(data);
        }
    }
}
