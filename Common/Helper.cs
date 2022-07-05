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
        public static void ClearBitInWires(List<IConnector> wires)
        {
            foreach (Wire item in wires)
            {
                item.DataInTransmission = null;
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

        public static int Binary_To_Int(string data)
        {
            char[] array = data.ToCharArray();
            int ans = 0;

            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] == '1')
                {
                    ans += (int)Math.Pow(2, array.Length - i - 1);
                }
            }
            return ans;
        }
        /// <summary>
        /// Recibe un entero y devuelve un string con su equivalente en binario 
        /// </summary>
        /// <param name="data">Entero que se quiere llevar a binario</param>
        /// <param name="length">Longitud del string que se devolverá</param>
        /// <returns></returns>
        public static string Int_To_Binary(int data, int length)
        {
            string result = "";
            while (data > 0)
            {
                int reminder = data % 2;
                if (reminder == 0)
                    result = "0" + result;
                else if (reminder == 1)
                    result = "1" + result;
                data /= 2;
            }
            while (result.Length < length)
            {
                result="0" + result;
            }
            return result;
        }

        public static string Binary_To_Hex(string data)
        {
            string result = "";
            for (int i = 0; i < data.Length; i += 4)
            {
                int aux = Binary_To_Int(data.Substring(i, 4));
                if (aux < 10)
                {
                    result += aux.ToString();
                }
                else
                {
                    switch (aux)
                    {
                        case 10:
                            result += "A";
                            break;
                        case 11:
                            result += "B";
                            break;
                        case 12:
                            result += "C";
                            break;
                        case 13:
                            result += "D";
                            break;
                        case 14:
                            result += "E";
                            break;
                        case 15:
                            result += "F";
                            break;
                        default:
                            throw new Exception("Binary Data out of range");
                    }
                }
            }
            return result;
        }

        public static Device Adjacent_Port(Port port, Dictionary<string, Device> devices)
        {
            IConnector wire = port.Connector;
            string device_name;
            if (wire.A.Name != port.Name)
            {
                device_name = wire.A.Name;
            }
            else
            {
                device_name = wire.A.Name;
            }
            device_name = device_name.Substring(0, device_name.Length - 2);
            Device result = devices[device_name];
            return result;
        }

        public static string Hex_To_Binary(string data)
        {
            string result = "";
            for (int i = 0; i < data.Length; i++)
            {
                char temp = data[i];
                switch (temp)
                {
                    case 'A':
                        result = result + Int_To_Binary(10,4);
                        break;
                    case 'B':
                        result = result + Int_To_Binary(11,4);
                        break;
                    case 'C':
                        result = result + Int_To_Binary(12,4);
                        break;
                    case 'D':
                        result = result + Int_To_Binary(13,4);
                        break;
                    case 'E':
                        result = result + Int_To_Binary(14,4);
                        break;
                    case 'F':
                        result = result + Int_To_Binary(15,4);
                        break;
                    default:
                        string aux = Int_To_Binary(int.Parse(temp.ToString()),4);
                        result = result + aux;
                        break;
                }
            }
            return result;
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

        public static string StringExtractor(List<Data> datas, int min, int max)
        {
            string result = "";
            for (int i = min; i < max; i++)
            {
                result+= datas[i].ToString();
            }
            return result;
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

        public static List<Data> String_To_DataList(string datas)
        {
            List<Data> result = new List<Data>();
            foreach(char c in datas)
            {
                if (c != '1' && c != '0')
                {
                    throw new Exception("Invalid datas format");
                }
                else
                {
                    result.Add(new Data(int.Parse(c.ToString())));
                }
            }
            return result;
        }
    }
}
