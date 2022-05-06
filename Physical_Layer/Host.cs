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
        public Host(string name)
        {
            Name = name;
            Port p = new LAN_Port(name + "_1");
            Ports = new List<Port>(1);
            Ports.Add(p);
        }

        public override void ReadData(int time)
        {
            Data newData = ((Simple_Wire)Ports[0].Connector).BitOnWire;
            if (newData != Ports[0].DataInPort)
            {
                PhysicalL_Writer.Write_File( time, Name, Ports[0].Name, "receive", (int)newData.Voltage, false);
                Ports[0].Put_Bit_In_Port(newData);
            }
        }
    }
}
