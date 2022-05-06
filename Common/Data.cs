using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum Voltage
    {
        Zero,
        One,
        Null,
        Interference
    }
    public class Data
    {
        public Voltage Voltage { get; set; }
        public Data(Voltage volt)
        {
            this.Voltage = volt;
        }
        
    }
}
