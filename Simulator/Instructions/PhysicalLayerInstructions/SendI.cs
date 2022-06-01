using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;
using Network_Simulator;

namespace Network_Simulator.Instructions
{
    public class SendI : Instruction
    {
        private Queue<Device> receivers;
        private Queue<Device> transmitters;

        public SendI(int time, string[] args) : base(time, args)
        {
            receivers = new Queue<Device>();
            transmitters = new Queue<Device>();
        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            Dictionary<string, List<Device>> adj = Helper.Get_AdjacencyList(devices, connectors);
            string data = Args[1];

            int pointer = 0;
            while (pointer < data.Length)
            {
                transmitters.Enqueue(devices[Args[0]]);
                Dictionary<string, int> d = Helper.Get_Negd(devices);
                d[transmitters.Peek().Name] = 0;

                while (transmitters.Count > 0)
                {
                    Device transmitter = transmitters.Dequeue();
                    foreach (Device v in adj[transmitter.Name])
                    {
                        if (d[v.Name] == -1)
                        {
                            d[v.Name] = d[transmitter.Name] + 1;
                            receivers.Enqueue(v);
                        }
                    }
                    transmitter.OnBitSent += new BitSent(ReadBit);
                    Data toSend = new Data(int.Parse(data[pointer].ToString()));
                    transmitter.SendData(toSend, Simulator.signal_time); ;
                }
                pointer++;
                Helper.ClearBitInWires(connectors);
            }            
        }

        private void ReadBit(Data bit)
        {
            foreach(Device dev in receivers)
            {
                dev.ReadData(Exec_Time);
            }
            while (receivers.Count > 0)
            {
                transmitters.Enqueue(receivers.Dequeue());
            }
        }
    }
}
