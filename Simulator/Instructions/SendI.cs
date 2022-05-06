using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;

namespace Simulator.Instructions
{
    public class SendI : Instruction
    {
        private bool control;

        public int Pointer { get; set; }
        public Data Data { get; set; }

        public SendI(int time, string[] args) : base(time, args)
        {
            Pointer = 0;
            int volt = int.Parse(Args[1][Pointer].ToString());
            Data = new Data((Voltage)volt);
            control = true;
        }

        public override void Exec(Dictionary<string, Device> devices, List<Wire> wires)
        {
            Device transmitter = devices[Args[0]];
            if (control)
            {   //Aqui supuestamente el emisor escribe en su txt, hay que ver si escribimos en otro lugar o aqui mismo
                //Tools.Write_File("Data\\" + transmitter.Name + ".txt", nc.Time, transmitter.Name, transmitter.Ports[0].Name, "send", Data.Value, false);
            }
            control = false;
            ///el bfs pone en los cables que alcanza el dato que se esta transmitiendo
            BFS(devices, wires, transmitter, Data);
        }
        private static void BFS(Dictionary<string, Device> devices, List<Wire> wires, Device start, Data data)
        {
            Dictionary<string, bool> visited = Helper.Get_Falsev(devices);
            Dictionary<string, int> d = Helper.Get_Negd(devices);
            Dictionary<string, Device> pi = Helper.Get_NullPi(devices);

            Dictionary<string, List<Device>> adj = Helper.Get_AdjacencyList(devices, wires);

            Queue<Device> q = new Queue<Device>();
            q.Enqueue(start);
            visited[start.Name] = true;
            d[start.Name] = 0;
            pi[start.Name] = null;

            while (q.Count != 0)
            {
                Device u = q.Dequeue();
                foreach (Device v in adj[u.Name])
                {
                    if (d[v.Name] == -1)
                    {
                        //aqui descubrimos un nuevo nodo hay que ver que se hace, por ahora poner Data en el cable(arista)
                        //mas adelante el dato se envia a una pc en especifico, en ese caso cuando la encontremos paramos
                        d[v.Name] = d[u.Name] + 1;
                        pi[v.Name] = u;
                        q.Enqueue(v);
                        Wire w = Helper.Get_Wire(u.Name, v.Name, wires);
                        w.BitOnWire = data;
                        v.ReadData(Time);///Time es el tiempo simulado del proyecto, hay que ver donde se pone
                    }
                }
            }
        }
    }
}
