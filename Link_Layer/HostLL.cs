using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physical_Layer;
using Common;
using Link_Layer.Interfaces;
using System.Configuration;

namespace Link_Layer
{
    public class HostLL : Host, ILinkLayerDev
    {
        //public event FrameOnDestination OnFrameOnDestination;
        private int data_counter;
        private int frame_length;
        private List<Data> Datas_Recived;
        public int signal_time;

        public MAC_Address MAC { get; private set; }
        public Frame Frame_Recived { get; set; }

        public HostLL(string name, int current_time) : base(name, current_time)
        {
            Datas_Recived = new List<Data>();
            data_counter = 0;
            signal_time = 10;
        }

        public void SetMAC(string mAC_Address)
        {
            if (mAC_Address.Length > 16)
            {
                throw new ArgumentOutOfRangeException("MAC legth is out of range");
            }
            if (MAC == null)
            {
                MAC = new MAC_Address(mAC_Address);
            }
        }

        public void ReadFrame(int port_number)
        {
            MAC_Address mac1 = new MAC_Address(Helper.Binary_To_Hex(Helper.StringExtractor(Datas_Recived, 0, 16)));
            if (mac1.CompareTo(MAC) == 0 || mac1.Address == "FFFF")
            //Si la MAC es FFFF significa que esta dirigida a todo el mundo (broadcast)
            {
                MAC_Address mac2 = new MAC_Address(Helper.Binary_To_Hex(Helper.StringExtractor(Datas_Recived, 16, 32)));
                string data_length = Helper.StringExtractor(Datas_Recived, 32, 40);
                int aux = Helper.Binary_To_Int(data_length);
                string verification_length = Helper.StringExtractor(Datas_Recived, 40, 48);
                int aux2 = Helper.Binary_To_Int(verification_length);
                string binary_data = Helper.StringExtractor(Datas_Recived, 48, 48 + aux * 8);
                string datas = Helper.Binary_To_Hex(binary_data);
                string verification = Helper.StringExtractor(Datas_Recived, 48 + aux * 8, 48 + aux * 8 + aux2);

                Frame_Recived = new Frame(mac1, mac2, data_length, verification_length, datas, verification);
                CRC_Protocol crc = new CRC_Protocol();
                bool error = int.Parse(crc.Division_Reminder(binary_data + verification)) != 0;

                LinkL_Writer.Write_File(Clock, Name, Frame_Recived.Transmiter, Frame_Recived.Datas, error);
            }

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

            SendDatas(Helper.String_To_DataList(datas));
        }

        public override bool SaveData()
        {
            bool sucessfullySaved = base.SaveData();
            if (sucessfullySaved)
            {
                Datas_Recived.Add(dataReceived);
                data_counter++;
                if (data_counter == 48)
                {
                    string data_length = Helper.StringExtractor(Datas_Recived, 32, 40);
                    int aux = Helper.Binary_To_Int(data_length);
                    string verification_length = Helper.StringExtractor(Datas_Recived, 40, 48);
                    int aux2 = Helper.Binary_To_Int(verification_length);
                    frame_length = 48 + aux * 8 + aux2;
                }
                if (data_counter == frame_length)
                {
                    ReadFrame(1);
                    data_counter = 0;
                    frame_length = 0;
                    Datas_Recived = new List<Data>();
                }
            }
            return sucessfullySaved;
        }
    }
}
