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

        public void ReadData(int time)
        {
            throw new NotImplementedException();
        }
    }
}
