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
            Ports.Add(p);
        }

        public override event BitSent OnBitSent;
        public override event DataSent OnDataSent;

        public override void ReadData(int time)
        {
            Data newData = ((Simple_Wire)Ports[0].Connector).BitOnWire;
            if (newData != Ports[0].DataInPort)
            {
                PhysicalL_Writer.Write_File(time, Name, Ports[0].Name, "receive", (int)newData.Voltage, false);
                Ports[0].Put_Bit_In_Port(newData);
            }
        }

        public override void SendData(Data data, int signal_time)
        {
            int i_time = signal_time;//este int es un clone de signal_time que se puede modificar sin afectar a signal_time
            while (i_time > 0)
            {
                if (p.Connector != null && ((Wire)p.Connector).BitOnWire == null)
                {
                    p.Put_Bit_In_Port(data);
                    IConnector aux = p.Connector;
                    ((Wire)aux).BitOnWire = data;
                    PhysicalL_Writer.Write_File(12, Name, p.Name, "send", data.Voltage, false);
                }
                i_time--;
            }
            OnBitSent(data);
        }
    }
}
