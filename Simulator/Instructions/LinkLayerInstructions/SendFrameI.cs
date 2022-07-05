using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link_Layer;
using Physical_Layer;
using Link_Layer.Interfaces;

namespace Network_Simulator.Instructions
{



    public class SendFrameI : Instruction
    {
        private Frame frame;

        public SendFrameI(int time, string[] args) : base(time, args) { }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            frame = Build_Frame(devices);
            ILinkLayerDev transmitter = (ILinkLayerDev)devices[Args[0]];
            transmitter.SendFrame(frame);
        }
        private Frame Build_Frame(Dictionary<string, Device> devices)
        {
            Device dev = devices[Args[0]];
            MAC_Address transmiter = ((HostLL)dev).MAC;
            string data_length = Helper.Int_To_Binary((Args[2].Length) / 2, 8);
            string data = Args[2];
            CRC_Protocol crc = new CRC_Protocol();
            string verification = crc.Generate_Verification_Data(Helper.Hex_To_Binary(data));
            string verification_length = Helper.Int_To_Binary(verification.Length, 8);
            Frame frame = new Frame(new MAC_Address(Args[1]), transmiter, data_length, verification_length, data, verification);//Cambiar despues el codigo de verificacion
            return frame;
        }
    }
}