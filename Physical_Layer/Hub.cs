using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{
    public class Hub : Device
    {
        private string v1;
        private int v2;

        public Hub(string v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}
