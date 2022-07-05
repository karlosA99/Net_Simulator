using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link_Layer
{
    public class MAC_Address : IComparable<MAC_Address>
    {
        public string Address { get; }

        public MAC_Address(string address)
        {
            if (address.Length == 4)
            {
                Address = address;
            }
            else
                throw new ArgumentOutOfRangeException("MAC_Address most have a length=4");
        }

        public int CompareTo(MAC_Address other)
        {
            return Address.CompareTo(other.Address);
        }

        //private bool[] String_To_Bytes(string address)
        //{
        //    bool[] aux = new bool[address.Length];
        //    for (int i = 0; i < address.Length; i++)
        //    {
        //        int temp = int.Parse(address[i].ToString());
        //        aux[i] = true ? temp == 1 : false;
        //    }
        //    return aux;
        //}
    }
}
