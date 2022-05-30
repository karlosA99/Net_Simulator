using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link_Layer;

namespace Network_Simulator.Instructions
{
    public class CreateSwitchI : Instruction
    {
        public CreateSwitchI(int time, string[] args) : base(time, args)
        {
        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            Switch h = new Switch(Args[0], int.Parse(Args[1]));
            devices.Add(h.Name, h);
        }
    }
}
