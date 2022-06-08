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
        /// <summary>   
        /// Cuantas Datas se han recibido
        /// </summary>
        private int data_counter;

        /// <summary>
        /// Longitud del frame que se esta leyendo
        /// </summary>
        private int frame_length;

        /// <summary>
        /// Lista con todos los Datas que se han recibido
        /// </summary>
        private List<Data>[] buffers;

        /// <summary>
        /// Puero por el que se recibio la ultima Data
        /// </summary>
        private Port receiving_port;

        /// <summary>
        /// Signal Time previamente configurado en App.config
        /// </summary>
        public int signal_time;

        /// <summary>
        /// Data que se esta enviando
        /// </summary>
        private Data data_sending;

        /// <summary>
        /// Almacena la relacion MAC destino con el puerto por el que se llega al dispositivo con esa MAC
        /// </summary>
        private Dictionary<string, Port> knewMACs;

        /// <summary>
        /// Almacena la relacion MAC destino con el puerto por el que se llega al dispositivo con esa MAC
        /// </summary>
        private Dictionary<Port, int> port_to_int;


        public override event DataSent OnDataSent;

        public Frame Frame_Recived { get; set; }

        public Switch(string name, int ports_count)
        {
            Name = name;
            Ports = new List<Port>(ports_count);
            port_to_int = new Dictionary<Port, int>();
            buffers = new List<Data>[ports_count];
            LAN_Port p;
            for (int i = 1; i <= ports_count; i++)
            {
                p = new LAN_Port(name + "_" + i);
                p.OnDataReceived += new DataReceived(ReadData);
                Ports.Add(p);
                port_to_int.Add(p, i - 1);
                buffers[i-1] = new List<Data>();
            }
            knewMACs = new Dictionary<string, Port>();
            
            signal_time = 10;
        }

        public Port PortOfMAC(string destination)
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
            if (!knewMACs.ContainsKey(mac.Address))
            {
                knewMACs.Add(mac.Address, port);
            }
        }


        public override void ReadData(Data data, Port port)
        { 
            receiving_port = port;
            int port_number = port_to_int[port];
            PhysicalL_Writer.Write_File(Clock, Name, Ports[0].Name, "receive", data.Voltage, false);
            buffers[port_number].Add(data);
            data_counter++;
            if (data_counter == 48)
            {
                string data_length = StringExtractor(32, 40, port_number);
                int aux = Helper.Binary_To_Int(data_length);
                string verification_length = StringExtractor(40, 48, port_number);
                int aux2 = Helper.Binary_To_Int(verification_length);
                frame_length = 48 + aux * 8 + aux2;
            }
            if (data_counter == frame_length)
            {
                ReadFrame(port_number);
                data_counter = 0;
                frame_length = 0;
                buffers[port_number] = new List<Data>();
            }
        }
        public override void SendData(Data data)
        {
            foreach (Port p in Ports)
            {
                if (!p.Equals(receiving_port) && p.Connector != null)
                {
                    PhysicalL_Writer.Write_File(Clock, Name, p.Name, "send", data.Voltage, false);
                    p.Put_Bit_In_Port(data);
                }
            }
        }
        public void SendData(Data data, int exec_time, Port p)
        {
            PhysicalL_Writer.Write_File(Clock, Name, p.Name, "send", data.Voltage, false);
            p.Put_Bit_In_Port(data);
            OnDataSent();
        }

        public void ReadFrame(int port_number)
        {
            MAC_Address mac1 = new MAC_Address(Helper.Binary_To_Hex(StringExtractor(0, 16, port_number)));
            MAC_Address mac2 = new MAC_Address(Helper.Binary_To_Hex(StringExtractor(16, 32, port_number)));
            string data_length = StringExtractor(32, 40, port_number);
            int aux = Helper.Binary_To_Int(data_length);
            string verification_length = StringExtractor(40, 48, port_number);
            int aux2 = Helper.Binary_To_Int(verification_length);
            string datas = Helper.Binary_To_Hex(StringExtractor(48, 48 + aux * 8, port_number));
            string verification = StringExtractor(48 + aux * 8, 48 + aux * 8 + aux2, port_number);

            Frame_Recived = new Frame(mac1, mac2, data_length, verification_length, datas, verification);

            AddMAC(mac2, receiving_port);

            SendFrame(Frame_Recived);
        }
        private string StringExtractor(int index1, int index2, int port_number)
        {
            string result = "";
            for (int i = index1; i < index2; i++)
            {
                result += buffers[port_number][i].Voltage.ToString();
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

            Port p = PortOfMAC(frame.Receiver.Address);
            if (p!=null)
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    Data data = new Data(int.Parse(datas[i].ToString()));
                    for (int j = signal_time; j > 0; j--)
                    {
                        if (data_sending == null || !data_sending.Equals(data))
                        {
                            data_sending = data;
                            SendData(data);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    Data data = new Data(int.Parse(datas[i].ToString()));
                    for (int j = signal_time; j > 0; j--)
                    {
                        if (data_sending == null || !data_sending.Equals(data))
                        {
                            data_sending = data;
                            SendData(data);
                        }
                    }
                }
            }


        }
    }
}
