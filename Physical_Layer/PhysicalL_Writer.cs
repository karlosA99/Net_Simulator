using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{
    public class PhysicalL_Writer
    {
        public static void Write_File(int time, string device_name, string port_name, string action, int voltage, bool collision)
        {
            string collisionSTR = "ok";
            if (collision)
            {
                collisionSTR = "collision";
            }
            string path = "Physical_Layer_Data\\" + device_name + ".txt";
            string info = time.ToString() + " " + port_name + " " + action + " " + voltage.ToString() + " " + collisionSTR;
            Helper.Write_File(path, info);
        }
    }
}
