using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer.Interfaces
{
    public interface ISenderDevice
    {
        void Update();
        bool SaveData();
        bool IsActive();
    }
}
