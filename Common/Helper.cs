using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        public static void Write_File(string path, string info)
        {
            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(info);
            sw.Close();
        }
        public static List<string> Read_File(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<string> lines = new List<string>();
            string line = sr.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            return lines;

        }

        public static int Binary_To_Int(string data_size)
        {
            char[] array = data_size.ToCharArray();
            int ans = 0;

            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] == '1')
                {
                    ans += (int)Math.Pow(2, i);
                }
            }
            return ans;
        }

        public static IConnector Get_Wire(string name1, string name2, List<IConnector> connectors)
        {
            foreach (var item in connectors)
            {
                string host1 = item.A.Name.Split('_')[0];
                string host2 = item.B.Name.Split('_')[0];
                if ((host1 == name1 && host2 == name2) || (host1 == name2 && host2 == name1))
                {
                    return item;
                }
            }
            return null;
        }

        public static Dictionary<string, Device> Get_NullPi(Dictionary<string, Device> devices)
        {
            Dictionary<string, Device> pi = new Dictionary<string, Device>();
            foreach (string item in devices.Keys)
            {
                pi.Add(item, null);
            }
            return pi;
        }

        public static Dictionary<string, int> Get_Negd(Dictionary<string, Device> devices)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            foreach (string item in devices.Keys)
            {
                d.Add(item, -1);
            }
            return d;
        }

        public static Dictionary<string, bool> Get_Falsev(Dictionary<string, Device> devices)
        {
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (string item in devices.Keys)
            {
                visited.Add(item, false);
            }
            return visited;
        }

        public static Dictionary<string, List<Device>> Get_AdjacencyList
                                                            (Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            Dictionary<string, List<Device>> adj = new Dictionary<string, List<Device>>();
            string name_d1;
            string name_d2;
            foreach (string item in devices.Keys)
            {
                adj.Add(item, new List<Device>());
            }
            foreach (Wire item in connectors)
            {
                name_d1 = item.A.Name.Split('_')[0];
                name_d2 = item.B.Name.Split('_')[0];
                adj[name_d1].Add(devices[name_d2]);
                adj[name_d2].Add(devices[name_d1]);
            }
            return adj;
        }



    }
}
