using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Instructions
{
    internal abstract class Instruction
    {
        public int Time { get; set; }
        public string[] Args { get; set; }
        public Instruction(int time, string[] args)
        {
            Time = time;
            Args = args;
        }
        public abstract void Exec(Net_Components nc);
    }
}
