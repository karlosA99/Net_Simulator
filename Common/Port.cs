using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class Port
    {
        
        public string Name { get; }
        public IConnector Connector { get; set; }
        public Data DataInPort { get; private set; }

        public Port(string name)
        {
            this.Name = name;
            DataInPort = null;
            Connector = null;

        }
        //public void Disconnect()
        //{
        //    if (Connector != null)
        //    {
        //        Connector = null;
        //    }
        //}
        
    }
}
