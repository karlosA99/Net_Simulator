using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulator.Instructions;

namespace Simulator
{
    public class Network_Simulator
    {
        public int signal_time;
        private bool not_finished;

        public Net_Components NC { get; set; }
        public string Path { get; set; }
        internal         Queue<Instruction> Instructions { get; set; }

        public Network_Simulator()
        {
            NC = new Net_Components();
            signal_time = 2;
            not_finished = true;
        }

        public void Run_Simulation()
        {
            while (not_finished)
            {

            }
        }
    }
}
