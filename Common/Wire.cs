using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    
    public abstract class Wire : IConnector
    {
        public Data BitOnWire { get; set; }
        
        public Port A { get; set; }
        public Port B { get; set; }

        public Wire(Port a, Port b)
        {
            this.A = a;
            this.B = b;
        }
    }
}
