using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;

namespace Link_Layer
{
    public class Duplex_Wire : Wire
    {
        Simple_Wire Channel1 { get; set; }
        Simple_Wire Channel2 { get; set; }
        public Duplex_Wire(Port a, Port b) : base(a, b)
        {
            Channel1 = new Simple_Wire(a, b);
            Channel2 = new Simple_Wire(b, a);
        }
    }
}
