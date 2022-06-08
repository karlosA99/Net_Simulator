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
        public override event DataSent OnDataSent;
        private Port p;
        private Data data_sending;
        public Host(string name)
        {
            Name = name;
            p = new LAN_Port(name + "_1");
            Ports = new List<Port>(1);
            p.OnDataReceived += new DataReceived(ReadData);
            Ports.Add(p);
        }

        public override void ReadData(Data data, Port p)
        {
            PhysicalL_Writer.Write_File(Clock, Name, Ports[0].Name, "receive", data.Voltage, false);
        }

        public override void SendData(Data data)
        {
            if(data_sending==null || !data_sending.Equals(data))
            {
                data_sending = data;
                PhysicalL_Writer.Write_File(Clock, Name, p.Name, "send", data.Voltage, false);
                p.Put_Bit_In_Port(data);
            }
            OnDataSent();
            //aqui comprobar si hubo collision haciendo XOR con el cable
        }
    }
}
