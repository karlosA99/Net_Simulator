using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public class Wire : IConnector
    {
        public Data DataInTransmission { get; set; }

        public bool InCollition { get; set; }

        public Port A { get; set; }
        public Port B { get; set; }

        public Wire(Port a, Port b)
        {
            this.A = a;
            this.B = b;
            //a.OnDataInPort += new DataInPort(ReceiveData);
            //b.OnDataInPort += new DataInPort(ReceiveData);
        }

        public virtual void ReceiveData(Data data, Port p)
        {
            if (DataInTransmission == null || !DataInTransmission.Equals(data))
                DataInTransmission = data;
            if (A.Equals(p))
                B.ReceiveData(data);
            else
                A.ReceiveData(data);
        }

        public virtual void ReceiveCollition(Port p)
        {
            InCollition = true;
            if (A.Equals(p))
                B.ReceiveCollition();
            else
                A.ReceiveCollition();
        }

        public virtual bool IsEmpty(Port p)
        {
            return DataInTransmission == null;
        }

        public virtual void CleanConnector()
        {
            DataInTransmission = null;
            InCollition = false;
        }
    }
}