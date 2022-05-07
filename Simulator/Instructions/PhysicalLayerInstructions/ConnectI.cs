using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;

namespace Network_Simulator.Instructions
{
    public class ConnectI : Instruction
    {
        public ConnectI(int time, string[] args) : base(time, args) { }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            string[] aux = Args[0].Split('_');
            Device device = devices[aux[0]];
            Port port1 = device.Ports[int.Parse(aux[1]) - 1];

            string[] aux2 = Args[1].Split('_');
            device = devices[aux2[0]];
            Port port2 = device.Ports[int.Parse(aux2[1]) - 1];

            Wire wire = new Simple_Wire(port1, port2);
            port1.Connector = wire;
            port2.Connector = wire;
            connectors.Add(wire);
        }
    }
}
