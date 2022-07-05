using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physical_Layer;
using Common;
using Physical_Layer.Interfaces;
using Link_Layer.Interfaces;

namespace Link_Layer
{
    public class Switch : Device, ILinkLayerDev, ISenderDevice
    {
        /// <summary>   
        /// Cuantas Datas se han recibido
        /// </summary>
        private int[] datas_counter;

        /// <summary>
        /// Longitud del frame que se esta leyendo
        /// </summary>
        private int[] frames_length;

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
        private PhysicalL_Sender[] pLSenders;
        private int[] timesToRead;
        private Data[] datasReceived;
        private bool[] newDatas;


        public override event DataSent OnDataSent;

        public Frame Frame_Recived { get; set; }

        public Switch(string name, int ports_count, int currentTime)
        {
            Name = name;
            timesToRead = new int[ports_count];
            datasReceived = new Data[ports_count];
            newDatas = new bool[ports_count];
            frames_length = new int[ports_count];
            datas_counter = new int[ports_count];
            pLSenders = new PhysicalL_Sender[ports_count];
            Ports = new List<Port>(ports_count);
            port_to_int = new Dictionary<Port, int>();
            buffers = new List<Data>[ports_count];
            LAN_Port p;
            for (int i = 0; i < ports_count; i++)
            {
                p = new LAN_Port(name + "_" + (i + 1));
                p.OnDataReceived += new DataReceived(ReadData);
                Ports.Add(p);
                pLSenders[i] = new PhysicalL_Sender(p, currentTime);
                port_to_int.Add(p, i + 1);
                buffers[i] = new List<Data>();
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
            if (data != null)
            {
                int port_number = port_to_int[port] - 1;
                if (datasReceived[port_number] == null || (datasReceived[port_number] != null && !datasReceived[port_number].Equals(data)))
                {
                    datasReceived[port_number] = data;
                    newDatas[port_number] = true;
                    timesToRead[port_number] = pLSenders[port_number].Signal_Time - 1;
                    receiving_port = port;
                    buffers[port_number].Add(data);
                }
            }
            //PhysicalL_Writer.Write_File(Clock, Name, Ports[0].Name, "receive", data.Voltage, false);

            //data_counter++;
            //if (data_counter == 48)
            //{
            //    string data_length = StringExtractor(32, 40, port_number);
            //    int aux = Helper.Binary_To_Int(data_length);
            //    string verification_length = StringExtractor(40, 48, port_number);
            //    int aux2 = Helper.Binary_To_Int(verification_length);
            //    frame_length = 48 + aux * 8 + aux2;
            //}
            //if (data_counter == frame_length)
            //{
            //    ReadFrame(port_number);
            //    data_counter = 0;
            //    frame_length = 0;
            //    buffers[port_number] = new List<Data>();
            //}
        }


        public void ReadFrame(int port_number)
        {
            MAC_Address mac1 = new MAC_Address(Helper.Binary_To_Hex(Helper.StringExtractor(buffers[port_number], 0, 16)));
            MAC_Address mac2 = new MAC_Address(Helper.Binary_To_Hex(Helper.StringExtractor(buffers[port_number], 16, 32)));
            string data_length = Helper.StringExtractor(buffers[port_number], 32, 40);
            int aux = Helper.Binary_To_Int(data_length);
            string verification_length = Helper.StringExtractor(buffers[port_number], 40, 48);
            int aux2 = Helper.Binary_To_Int(verification_length);
            string datas = Helper.Binary_To_Hex(Helper.StringExtractor(buffers[port_number], 48, 48 + aux * 8));
            string verification = Helper.StringExtractor(buffers[port_number], 48 + aux * 8, 48 + aux * 8 + aux2);

            Frame_Recived = new Frame(mac1, mac2, data_length, verification_length, datas, verification);

            AddMAC(mac2, receiving_port);

            SendFrame(Frame_Recived);
        }

        public void SendFrame(Frame frame)
        {
            string datas = "";                                         //
            datas += Helper.Hex_To_Binary(frame.Receiver.Address);     //
            datas += Helper.Hex_To_Binary(frame.Transmiter.Address);   //Aqui se construye un string con toda la informacion del frame en 
            datas += frame.Data_Length;                                //binario para enviarse utilizando la capa fisica
            datas += frame.Verification_Length;                        //
            datas += Helper.Hex_To_Binary(frame.Datas);                //
            datas += frame.Verification;                               //

            Port p = PortOfMAC(frame.Receiver.Address);
            if (p != null)
            {
                foreach (PhysicalL_Sender pLS in pLSenders)
                {
                    if (pLS.Port.Equals(p) && pLS.Port.Connector != null)
                    {
                        pLS.AddPackage(Helper.String_To_DataList(datas));
                    }
                }
            }
            else
            {
                foreach (PhysicalL_Sender pLS in pLSenders)
                {
                    if (!pLS.Port.Equals(receiving_port) && pLS.Port.Connector != null)
                    {
                        pLS.AddPackage(Helper.String_To_DataList(datas));
                    }
                }
            }
        }

        public void Update()
        {
            foreach (PhysicalL_Sender pLS in pLSenders)
            {
                pLS.Update();
            }
        }

        public bool IsActive()
        {
            foreach (PhysicalL_Sender pLS in pLSenders)
            {
                if (pLS.IsActive())
                {
                    return true;
                }
            }
            return false;
        }

        public bool SaveData()
        {
            for (int i = 0; i < buffers.Length; i++)
            {
                if (timesToRead[i] == 0)
                {
                    if (datasReceived[i] != null && newDatas[i])
                    {
                        PhysicalL_Writer.Write_File(Clock, Name, Ports[0].Name, "receive", datasReceived[i].Voltage, false);
                        newDatas[i] = false;
                        timesToRead[i] = int.MaxValue;
                        datas_counter[i]++;
                        if (datas_counter[i] == 48)
                        {
                            string data_length = Helper.StringExtractor(buffers[i], 32, 40);
                            int aux = Helper.Binary_To_Int(data_length);
                            string verification_length = Helper.StringExtractor(buffers[i], 40, 48);
                            int aux2 = Helper.Binary_To_Int(verification_length);
                            frames_length[i] = 48 + aux * 8 + aux2;
                        }
                        if (datas_counter[i] == frames_length[i])
                        {
                            receiving_port = Ports[i];
                            ReadFrame(i);
                            datas_counter[i] = 0;
                            frames_length[i] = 0;
                            buffers[i] = new List<Data>();
                        }
                    }
                    timesToRead[i] = int.MaxValue;
                }
                else
                {
                    timesToRead[i]--;
                }
            }
            return true;
        }
    }
}
