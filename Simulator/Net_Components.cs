using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Simulator
{
    public class Net_Components
    {
        public Dictionary<string,Device> Devices { get; set; }
        public List<Wire> Wires { get; set; }
        public int Time { get; set; }
        public Net_Components()
        {
            Devices = new Dictionary<string,Device>();
            Wires = new List<Wire>();
        }
    }
}