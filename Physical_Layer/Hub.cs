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
            Simple_Wire wire_in_port = (Simple_Wire)receiving_port.Connector;
            if (receiving_port.DataInPort != wire_in_port.BitOnWire)
            {
                receiving_port.Put_Bit_In_Port(wire_in_port.BitOnWire);
                PhysicalL_Writer.Write_File(time, Name, receiving_port.Name, "receive", (int)receiving_port.DataInPort.Voltage, false);
                foreach (LAN_Port item in Ports)
                {
                    if (item.Connector != null)
                    {

                        if (receiving_port.Name != item.Name)
                        {
                            item.Put_Bit_In_Port(wire_in_port.BitOnWire);
                            PhysicalL_Writer.Write_File(time, Name, item.Name, "send", (int)receiving_port.DataInPort.Voltage, false);
                        }
                    }
                }
            }
        }
    }
}
