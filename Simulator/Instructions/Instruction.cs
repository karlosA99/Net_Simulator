using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Simulator.Instructions
{
    public abstract class Instruction
    {
        public int Time { get; }
        public string[] Args { get; }

        public Instruction(int time, string[] args)
        {
            Time = time;
            Args = args;
        }
        public abstract void Exec(Dictionary<string,Device> devices, List<Wire> wires);
    }
}
