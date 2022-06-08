using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;

namespace Link_Layer
{
    public class Duplex_Wire : Wire, IConnector
    {
        Simple_Wire ASending { get; set; }
        Simple_Wire BSending { get; set; }
        public Duplex_Wire(Port a, Port b) : base(a, b)
        {
            ASending = new Simple_Wire(a, b);
            BSending = new Simple_Wire(b, a);
        }
    }
}
