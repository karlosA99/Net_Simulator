using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;

namespace Network_Simulator.Instructions
{
    public class SendI : Instruction
    {

        public int Pointer { get; set; }
        private Data data;

        public SendI(int time, string[] args) : base(time, args)
        {
            Pointer = 0;
            int volt = int.Parse(Args[1][Pointer].ToString());
            data = new Data((Voltage)volt);
        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            Device transmitter = devices[Args[0]];
            PhysicalL_Writer.Write_File(Exec_Time, transmitter.Name, transmitter.Ports[0].Name, "send", (int)data.Voltage, false);

            while (Pointer != Args[1].Length)
            {
                int i_time = Simulator.signal_time;
                while (i_time > 0)
                {
                    ///el bfs pone en los cables que alcanza el dato que se esta transmitiendo
                    BFS(devices, connectors, transmitter, data);
                    i_time--;
                    Simulator.Time++;
                }
                Pointer++;
                if (Pointer < Args[1].Length)
                {
                    int volt = int.Parse(Args[1][Pointer].ToString());
                    data = new Data((Voltage)volt);
                }
            }
        }
        private void BFS(Dictionary<string, Device> devices, List<IConnector> connectors, Device start, Data data)
        {
            Dictionary<string, bool> visited = Helper.Get_Falsev(devices);
            Dictionary<string, int> d = Helper.Get_Negd(devices);
            Dictionary<string, Device> pi = Helper.Get_NullPi(devices);

            Dictionary<string, List<Device>> adj = Helper.Get_AdjacencyList(devices, connectors);

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
                        IConnector w = Helper.Get_Wire(u.Name, v.Name, connectors);
                        ((Wire)w).BitOnWire = data;
                        v.ReadData(Exec_Time);///Time es el tiempo simulado del proyecto, hay que ver donde se pone
                    }
                }
            }
        }
    }
}
