using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{
    internal class PhysicalL_Writer
    {
        internal static void Write_File(int time, string device_name, string port_name, string action, int voltage, bool collision)
        {
            string path = "Data\\" + device_name + ".txt";
            string info = time.ToString() + " " + port_name + " " + action + " " + voltage.ToString() + " " + collision;
            Helper.Write_File(path, info);
        }
    }
}
