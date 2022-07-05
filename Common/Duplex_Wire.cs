using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Common
{
    public class Duplex_Wire : Wire, IConnector
    {
        private Wire aSending;
        private Wire bSending;

        /// <summary>
        /// Bit On A Sending Wire
        /// </summary>
        public Data BOASW { get; set; }
        /// <summary>
        /// Bit On B Sending Wire
        /// </summary>
        public Data BOBSW { get; set; }


        public Duplex_Wire(Port a, Port b) : base(a, b)
        {
            aSending = new Wire(a, b);
            bSending = new Wire(b, a);
        }
        public override void ReceiveData(Data data, Port p)
        {
            if (aSending.A.Equals(p))
            {
                if(BOASW == null|| !BOASW.Equals(data))
                {
                    BOASW = data;
                }
                aSending.ReceiveData(data, p);
            }
            else
            {
                if (BOBSW == null || !BOBSW.Equals(data))
                {
                    BOBSW = data;
                }
                bSending.ReceiveData(data, p);
            }
        }
        public override void ReceiveCollition(Port p)
        {
            InCollition = true;
            if (aSending.A.Equals(p))
            {
                aSending.ReceiveCollition(p);
            }
            else
            {
                bSending.ReceiveCollition(p);
            }
        }

        public override bool IsEmpty(Port p)
        {
            if (aSending.A.Equals(p))
            {
                return aSending.IsEmpty(p);
            }
            else
            {
                return bSending.IsEmpty(p);
            }
        }
        /// <summary>
        /// Vacia los datos que este transmitiendo
        /// </summary>
        public override void CleanConnector()
        {
            InCollition = false;
            bSending.CleanConnector();
            aSending.CleanConnector();
        }
    }
}
