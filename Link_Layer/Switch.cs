using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physical_Layer;
using Common;
using Link_Layer.Interfaces;

namespace Link_Layer
{
    public class Switch : Hub, ILinkLayerDev
    {
        private List<Data> Bits_Recived;

        Dictionary<MAC_Address, Port> knewMACs;
        public Frame Frame_Recived { get; set; }

        public Switch(string name, int ports_count) : base(name, ports_count)
        {
            knewMACs = new Dictionary<MAC_Address, Port>();
            Bits_Recived = new List<Data>();
        }

        public Port PortOfMAC(MAC_Address destination)
        {
            if (knewMACs.ContainsKey(destination))
            {
                return knewMACs[destination];
            }
            else
            {
                return null;
            }
        }
        public void AddMAC(MAC_Address mac, Port port)
        {
            if (!knewMACs.ContainsKey(mac))
            {
                knewMACs.Add(mac, port);
            }
        }

        public void ReadFrame(int time)
        {
            MAC_Address mac1 = new MAC_Address(StringExtractor(0, 16));
            MAC_Address mac2 = new MAC_Address(StringExtractor(16, 32));
            string data_length = StringExtractor(32, 40);
            int aux = Helper.Binary_To_Int(data_length);
            string verification_length = StringExtractor(40, 48);
            int aux2 = Helper.Binary_To_Int(verification_length);
            string datas = StringExtractor(48, 48 + aux * 8);
            string verification = StringExtractor(48 + aux * 8, 48 + aux * 8 + aux2 * 8);

            Frame_Recived = new Frame(mac1, mac2, data_length, verification_length, datas, verification);
        }

        private string StringExtractor(int index1, int index2)
        {
            string result = "";
            for (int i = index1; i < index2; i++)
            {
                result += Bits_Recived[i].Voltage.ToString();
            }
            return Helper.Binary_To_Hex(result);
        }

        public void ReadBit(Data bit)
        {
            Bits_Recived.Add(bit);
        }
    }
}
