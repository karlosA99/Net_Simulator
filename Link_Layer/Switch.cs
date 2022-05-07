using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physical_Layer;
using Common;

namespace Link_Layer
{
    public class Switch : Hub
    {
        Dictionary<MAC_Address, Port> knewMACs;
        public Switch(string name, int ports_count) : base(name, ports_count)
        {
            knewMACs = new Dictionary<MAC_Address, Port>();
        }

        public Port PortOfMAC(MAC_Address destination)
        {
            if (knewMACs.ContainsKey(destination))
            {
                return knewMACs[destination];
            }
            else
            {
                return null;
            }
        }
        public void AddMAC(MAC_Address mac, Port port)
        {
            if (!knewMACs.ContainsKey(mac))
            {
                knewMACs.Add(mac, port);
            }
        }
    }
}
