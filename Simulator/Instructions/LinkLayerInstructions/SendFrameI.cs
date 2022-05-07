using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link_Layer;

namespace Network_Simulator.Instructions
{
    public class SendFrameI : Instruction
    {
        private Frame frame;
        public SendFrameI(int time, string[] args) : base(time, args)
        {

        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            throw new NotImplementedException();
        }
    }
}
