using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link_Layer
{
    public class MAC_Address
    {
        public bool[] Address { get; }

        public MAC_Address(string address)
        {
            Address = String_To_Bytes(address);
        }

        private bool[] String_To_Bytes(string address)
        {
            bool[] aux = new bool[address.Length];
            for (int i = 0; i < address.Length; i++)
            {
                int temp = int.Parse(address[i].ToString());
                aux[i] = true ? temp == 1 : false;
            }
            return aux;
        }
    }
}
