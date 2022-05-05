using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Helper
    {
        public static void ClearBitInWires(List<Wire> wires)
        {
            foreach (Wire item in wires)
            {
                item.BitOnWire = null;
                
            }
        }
        internal static void BFS(Dictionary<string, Device> devices, List<Wire> wires, Device start, Data data)
        {
            Dictionary<string, bool> visited = Get_Falsev(devices);
            Dictionary<string, int> d = Get_Negd(devices);
            Dictionary<string, Device> pi = Get_NullPi(devices);

            Dictionary<string, List<Device>> adj = Get_AdjacencyList(devices, wires);

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
                        Wire w = Get_Wire(u.Name, v.Name, wires);
                        w.Data = data;
                        v.ReadData(nc.Time);
                    }
                }
            }
        }
    }
}
