using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link_Layer;

namespace Network_Simulator.Instructions
{
    public class CreateSwitchI : CreateI
    {
        public CreateSwitchI(int time, string[] args) : base(time, args)
        {
        }

        public override event NewDevice OnNewDevice;

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            Switch h = new Switch(Args[0], int.Parse(Args[1]));
            h.signal_time = Simulator.Signal_Time;
            devices.Add(h.Name, h);
            OnNewDevice(h);
        }
    }
}
