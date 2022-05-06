using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physical_Layer;
using Common;

namespace Simulator
{
    public class PLManager : IPLManager
    {
        public void ConnectPorts(string device1Name, int device1Port, string device2Name, int device2Port)
        {
            throw new NotImplementedException();
        }

        public void CreateHost(string name)
        {
            throw new NotImplementedException();
        }

        public void CreateHub(string name, int ports)
        {
            throw new NotImplementedException();
        }

        public void DisconnectPort(string deviceName, int devicePort)
        {
            throw new NotImplementedException();
        }

        public void Send(string hostName, Data data)
        {
            throw new NotImplementedException();
        }
    }
}
