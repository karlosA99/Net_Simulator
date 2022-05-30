using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum Voltage
    {
        zero,
        One,
        Null,
        Interference
    }
    public class Data
    {
        public int Voltage { get; set; }
        public Data(int volt)
        {
            if(volt != 0 && volt != 1)
            {
                throw new ArgumentOutOfRangeException("Voltage is only 0 or 1");
            }
            this.Voltage = volt;
        }
        
    }
}
