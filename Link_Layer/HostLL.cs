using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physical_Layer;

namespace Link_Layer
{
    public class HostLL : Host
    {
        public MAC_Address MAC { get; private set; }
        public HostLL(string name) : base(name)
        {
        }

        public void SetMAC(string mAC_Address)
        {
            if(MAC == null)
            {
                MAC = new MAC_Address(mAC_Address); 
            }
        }
    }
}
