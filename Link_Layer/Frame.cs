using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Link_Layer
{
    public class Frame
    {
        public MAC_Address Transmiter { get;}
        public MAC_Address Receiver { get;}
        public string Data_Length { get; }
        public string Verification_Length { get; }
        public string Datas { get; }
        public string Verification { get; }

        public Frame(MAC_Address receiver, MAC_Address transmiter,string data_length,string verification_length, string data, string verification_data)
        {
            Transmiter = transmiter;
            Receiver = receiver;

            if (data_length.Length > 8)
                throw new ArgumentOutOfRangeException("Data length is out of range");
            else
            {
                while (data_length.Length < 8)
                {
                    data_length = "0" + data_length;
                }
                Data_Length = data_length;
            }
                

            if(verification_length.Length > 8)
                throw new ArgumentOutOfRangeException("Verification length is out of range");
            else
                Verification_Length = verification_length;

            if (Helper.Binary_To_Int(Data_Length) != data.Length/2)
                throw new Exception("Data_Length is diferent from Data.Legth");
            else
                Datas = data;
            Verification = verification_data;
        }
    }
}
