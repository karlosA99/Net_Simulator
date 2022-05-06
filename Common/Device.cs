using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common
{
    public abstract class Device
    {
        public string Name { get; set; }
        public List<Port> Ports { get; set; } 
        public abstract void ReadData(int time);
    }
}
