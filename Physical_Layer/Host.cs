using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{
    public class Host : Device
    {
        public Host(string name)
        {
            this.Name = name;
            Port a = new LAN_Port(name + "_1");
        }
    }
}
