using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common
{
    //public delegate void DataSent();
    //public delegate void BitSent(Data bit);

    public abstract class Device
    {
        //public abstract event BitSent OnBitSent;
        //public abstract event DataSent OnDataSent;
        public string Name { get; set; }
        public List<Port> Ports { get; set; }
        public abstract void ReadData(Data data, Port port);

        public abstract void SendData(Data data, int exec_time);
    }
}
