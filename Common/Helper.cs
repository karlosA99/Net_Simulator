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
    }
}
