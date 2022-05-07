using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Link_Layer
{
    public class Frame
    {
        public MAC_Address Transmiter { get;}
        public MAC_Address Receiver { get;}
        public Data[] Datas { get; }
        public int[] Verification { get; }

        public Frame(string receiver, string transmiter, Data[] data, int[] verification_data)
        {
            Transmiter = new MAC_Address(transmiter);
            Receiver = new MAC_Address(receiver);
            Datas = data;
            Verification = verification_data;
        }
    }
}
