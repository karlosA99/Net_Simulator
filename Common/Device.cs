using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common
{
    public delegate void DataSent();
    public abstract class Device
    {
        public abstract event DataSent OnDataSent;
        public int Clock { get; set; }
        public string Name { get; set; }
        public List<Port> Ports { get; set; }

        public abstract void ReadData(Data data, Port port);
        public virtual void ClockTick()
        {
            Clock++;
        }
    }
}
