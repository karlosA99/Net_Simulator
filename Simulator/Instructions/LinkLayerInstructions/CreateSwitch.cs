using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Simulator.Instructions
{
    public class CreateSwitch : Instruction
    {
        public CreateSwitch(int time, string[] args) : base(time, args)
        {
        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            throw new NotImplementedException();
        }
    }
}
