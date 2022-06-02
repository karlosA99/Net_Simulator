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
    public class Switch : Device, ILinkLayerDev
    {
        private int data_counter;
        private int frame_length;
        private List<Data> Datas_Recived;
        private Port receiving_port;
        List<Port> transmitters_ports;

        Dictionary<MAC_Address, Port> knewMACs;
        public Frame Frame_Recived { get; set; }

        public Switch(string name, int ports_count)
        {
            Name = name;
            Ports = new List<Port>(ports_count);
            LAN_Port p;
            for (int i = 1; i <= ports_count; i++)
            {
                p = new LAN_Port(name + "_" + i);
                p.OnDataReceived += new DataReceived(ReadData);
                Ports.Add(p);
            }
            knewMACs = new Dictionary<MAC_Address, Port>();
            Datas_Recived = new List<Data>();
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


        public override void ReadData(Data data, Port port)
        {
            receiving_port = port;
            PhysicalL_Writer.Write_File(1, Name, Ports[0].Name, "receive", data.Voltage, false);
            Datas_Recived.Add(data);
            data_counter++;
            if (data_counter == 48)
            {
                string data_length = StringExtractor(32, 40);
                int aux = Helper.Binary_To_Int(data_length);
                string verification_length = StringExtractor(40, 48);
                int aux2 = Helper.Binary_To_Int(verification_length);
                frame_length = 48 + aux * 8 + aux2 * 8;
            }
            if (data_counter == frame_length)
            {
                ReadFrame(1);
                data_counter = 0;
                frame_length = 0;
                Datas_Recived = new List<Data>();
            }
        }
        public override void SendData(Data data, int exec_time)
        {
            foreach (Port p in Ports)
            {
                if (!p.Equals(receiving_port) && p.Connector != null)
                {
                    PhysicalL_Writer.Write_File(12, Name, p.Name, "send", data.Voltage, false);
                    p.Put_Bit_In_Port(data);
                }
            }
        }
        public void SendData(Data data, int exec_time, Port p)
        {
            PhysicalL_Writer.Write_File(12, Name, p.Name, "send", data.Voltage, false);
            p.Put_Bit_In_Port(data);
        }

        public void ReadFrame(int time)
        {
            MAC_Address mac1 = new MAC_Address(Helper.Binary_To_Hex(StringExtractor(0, 16)));
            MAC_Address mac2 = new MAC_Address(Helper.Binary_To_Hex(StringExtractor(16, 32)));
            string data_length = StringExtractor(32, 40);
            int aux = Helper.Binary_To_Int(data_length);
            string verification_length = StringExtractor(40, 48);
            int aux2 = Helper.Binary_To_Int(verification_length);
            string datas = Helper.Binary_To_Hex(StringExtractor(48, 48 + aux * 8));
            string verification = StringExtractor(48 + aux * 8, 48 + aux * 8 + aux2 * 8);

            Frame_Recived = new Frame(mac1, mac2, data_length, verification_length, datas, verification);

            SendFrame(Frame_Recived);
        }
        private string StringExtractor(int index1, int index2)
        {
            string result = "";
            for (int i = index1; i < index2; i++)
            {
                result += Datas_Recived[i].Voltage.ToString();
            }
            return result;
        }
        public void SendFrame(Frame frame)
        {
            string datas = "";
            datas += Helper.Hex_To_Binary(frame.Receiver.Address);
            datas += Helper.Hex_To_Binary(frame.Transmiter.Address);
            datas += frame.Data_Length;
            datas += frame.Verification_Length;
            datas += Helper.Hex_To_Binary(frame.Datas);
            datas += frame.Verification;

            transmitters_ports = new List<Port>();

            if (knewMACs.ContainsKey(frame.Receiver))
            {
                Port p = knewMACs[frame.Receiver];
                for (int i = 0; i < datas.Length; i++)
                {
                    Data data = new Data(int.Parse(datas[i].ToString()));
                    SendData(data, 1, p);
                }
            }
            else
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    Data data = new Data(int.Parse(datas[i].ToString()));
                    SendData(data, 1);
                }
            }


        }
    }
}
