using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Link_Layer
{
    public class LinkL_Writer
    {
        public static void Write_File(int time,string device_name, MAC_Address transmitter_mac, string data, bool Error)
        {
            string corrupted = "";
            if (Error)
                corrupted = "ERROR";

            string path = "Link_Layer_Data\\" + device_name + "_data.txt";
            string info = time.ToString() + " " + transmitter_mac.Address + " " + data + " " + corrupted;
            Helper.Write_File(path, info);
        }
    }
}
