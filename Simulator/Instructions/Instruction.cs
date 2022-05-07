using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Network_Simulator.Instructions
{
    public abstract class Instruction
    {
        public int Exec_Time { get;}
        public string[] Args { get; }
        
        public Instruction(int time, string[] args)
        {
            Exec_Time = time;
            Args = args;
        }

        public abstract void Exec(Dictionary<string,Device> devices, List<IConnector> connectors);
    }
}
