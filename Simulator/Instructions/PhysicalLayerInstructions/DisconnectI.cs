using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Network_Simulator.Instructions
{
    public class DisconnectI : Instruction
    {
        public DisconnectI(int time, string[] args) : base(time, args) { }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            foreach (Wire item in connectors)
            {
                if (item.A.Name == Args[0])
                {
                    item.A.Connector = null;
                    item.B.Connector = null;
                    connectors.Remove(item);
                    break;
                }
            }
        }
    }
}
