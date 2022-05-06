using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;

namespace Simulator.Instructions
{
    public class CreateHubI : Instruction
    {
        public CreateHubI(int time, string[] args) : base(time, args) { }

        public override void Exec(Dictionary<string, Device> devices, List<Wire> wires)
        {
            Hub h = new Hub(Args[0], int.Parse(Args[1]));
            devices.Add(h.Name, h);
        }
    }
}
