using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link_Layer.Interfaces
{
    internal interface IFrameBuilder
    {
        void Build(string raw_frame);
    }
}
