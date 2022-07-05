using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public delegate void DataReceived(Data data, Port port);
    public delegate void CollitionDetected(Port port);
    public abstract class Port
    {
        public event DataReceived OnDataReceived;
        public event CollitionDetected OnCollitionDetected;

        public string Name { get; }
        public IConnector Connector { get; set; }
        public Data DataInPort { get; private set; }

        public Port(string name)
        {
            this.Name = name;
            DataInPort = null;
            Connector = null;
        }
        public void Disconnect()
        {
            if (Connector != null)
            {
                Connector = null;
            }
        }
        public void Connect(Wire connector)
        {
            Connector = connector;
        }

        internal void ReceiveData(Data data)
        {
            DataInPort = data;
            OnDataReceived(data, this);
        }
        internal void ReceiveCollition()
        {
            OnCollitionDetected(this);
        }

        public void Put_Bit_In_Port(Data data)
        {
            DataInPort = data;
            if (data != null && !Connector.IsEmpty(this))
            {
                Connector.ReceiveCollition(this);
                OnCollitionDetected(this);
            }
            else
            {
                Connector.ReceiveData(data, this);
            }

        }

        public void SendCollitionMessage()
        {
            Connector.ReceiveCollition(this);
        }

        public bool Equals(Port obj)
        {
            return this.Name.Equals(obj.Name);
        }
    }
}
