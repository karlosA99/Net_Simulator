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
    public class HostLL : Host, ILinkLayerDev
    {
        //public event FrameOnDestination OnFrameOnDestination;
        private int data_counter;
        private int frame_length;
        private List<Data> Datas_Recived;

        public MAC_Address MAC { get; private set; }
        public Frame Frame_Recived { get; set; }
             
        public HostLL(string name) : base(name)
        {
            Datas_Recived = new List<Data>();
            data_counter = 0;
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

        public void ReadFrame(int time)
        {
            MAC_Address mac1 = new MAC_Address(Helper.Binary_To_Hex(StringExtractor(0, 16)));
            if (mac1.CompareTo(MAC) == 0 || mac1.Address=="FFFF")
            //Si la MAC es FFFF significa que esta dirigida a todo el mundo (broadcast)
            {
                MAC_Address mac2 = new MAC_Address(Helper.Binary_To_Hex(StringExtractor(16, 32)));
                string data_length = StringExtractor(32, 40);
                int aux = Helper.Binary_To_Int(data_length);
                string verification_length = StringExtractor(40, 48);
                int aux2 = Helper.Binary_To_Int(verification_length);
                string datas = Helper.Binary_To_Hex(StringExtractor(48, 48 + aux * 8));
                string verification = StringExtractor(48 + aux * 8, 48 + aux * 8 + aux2 * 8);

                Frame_Recived = new Frame(mac1, mac2, data_length, verification_length, datas, verification);

                LinkL_Writer.Write_File(time, Name, Frame_Recived.Transmiter, Frame_Recived.Datas, false);
            }

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

            for (int i = 0; i < datas.Length; i++)
            {
                Data data = new Data(int.Parse(datas[i].ToString()));
                SendData(data, 1);
            }
        }

        public override void ReadData(Data data, Port p)
        {
            base.ReadData(data, p);
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

    }
}
