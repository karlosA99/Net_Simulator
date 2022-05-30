using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Link_Layer.Interfaces
{
    public interface ILinkLayerDev
    {
        Frame Frame_Recived { get; set; }

        void ReadBit(Data bit);
        void ReadFrame(int time);
    }
}
