using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link_Layer;

namespace Network_Simulator.Instructions
{
    internal class SetMacI : Instruction
    {
        public SetMacI(int time, string[] args) : base(time, args)
        {
        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            Device device = devices[Args[0]];
            if(device is HostLL)
            {
                HostLL temp = (HostLL)device;
                temp.SetMAC(Args[1]);
            }
        }
    }
}
