using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;

namespace Simulator.Instructions
{
    public class CreateHostI : Instruction
    {
        public CreateHostI(int time, string[] args) : base(time, args) { }

        public override void Exec(Dictionary<string, Device> devices, List<Wire> wires)
        {
            Host h = new Host(Args[0]);
            devices.Add(h.Name, h);
        }
    }
}
