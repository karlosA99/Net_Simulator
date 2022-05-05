using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{
    public interface IPLManager
    {
        void ConnectPorts(string device1Name, int device1Port, string device2Name, int device2Port);
        void CreateHost(string name);
        void CreateHub(string name, int ports);
        void DisconnectPort(string deviceName, int devicePort);
        void Send(string hostName, Data data);
    }
}
