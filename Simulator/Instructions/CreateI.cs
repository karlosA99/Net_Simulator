using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Network_Simulator.Instructions
{
    public delegate void NewDevice(Device dev);
    public abstract class CreateI : Instruction
    {
        public abstract event NewDevice OnNewDevice;
        protected CreateI(int time, string[] args) : base(time, args)
        {
        }
    }
}
