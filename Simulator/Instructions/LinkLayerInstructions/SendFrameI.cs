﻿using Common;
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
    public delegate void BitRecived(Data bit);
    public delegate void DataSent(int time);

    public class SendFrameI : Instruction
    {
        private Frame frame;
        private Queue<Device> receivers;
        private Queue<Device> transmitters;
        public SendFrameI(int time, string[] args) : base(time, args) 
        {
            receivers = new Queue<Device>();
            transmitters = new Queue<Device>();
        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            frame = Build_Frame(devices);
            
            transmitters.Enqueue(devices[Args[0]]);
            Dictionary<string, List<Device>> adj = Helper.Get_AdjacencyList(devices, connectors);
            Dictionary<string, int> d = Helper.Get_Negd(devices);
            d[transmitters.Peek().Name] = 0;

            
            string arg2 = Helper.Hex_To_Binary(frame.Receiver.Address) + Helper.Hex_To_Binary(frame.Transmiter.Address) + frame.Data_Length + frame.Verification_Length + Helper.Hex_To_Binary(frame.Datas) + frame.Verification;

            while (transmitters.Count > 0)
            {
                Device transmitter = transmitters.Dequeue();
                string[] args = { transmitter.Name, arg2 };
                SendI sendI = new SendI(Exec_Time, args);
                

                if(transmitter is Switch)
                {
                    Switch aux = (Switch)transmitter;
                    if (aux.PortOfMAC(frame.Receiver) != null)
                    {
                        Device next = Helper.Adjacent_Port(aux.PortOfMAC(frame.Receiver),devices); 
                        receivers.Enqueue(next);
                    }
                }
                foreach (Device v in adj[transmitter.Name])
                {
                    if(d[v.Name] == -1)
                    {
                        d[v.Name] = d[transmitter.Name] + 1;
                        receivers.Enqueue(v);
                    }
                }
                sendI.OnBitRecived += new BitRecived(ReadBit);
                sendI.OnDataSent += new DataSent(ReadData);
                sendI.Exec(devices, connectors);
            }            
        }
        private Frame Build_Frame(Dictionary<string, Device> devices)
        {
            Device dev = devices[Args[0]];
            MAC_Address transmiter = ((HostLL)dev).MAC;
            string data_length = Helper.Int_To_Binary((Args[2].Length)/2);                                        
            string data = Args[2];                                                                       
            Frame frame = new Frame(new MAC_Address(Args[1]), transmiter, data_length, "00000001", data , "00000101");//Cambiar despues el codigo de verificacion
            return frame;
        }

        private void ReadData(int time)
        {
            while (receivers.Count > 0)
            {
                Device dev = receivers.Dequeue();
                if (dev is HostLL || dev is Switch)
                {
                    ((ILinkLayerDev)dev).ReadFrame(time);
                }
                transmitters.Enqueue(dev);
            }
        }
        private void ReadBit(Data bit)
        {
            foreach (var dev in receivers)
            {
                if (dev is ILinkLayerDev)
                {
                    ILinkLayerDev aux = (ILinkLayerDev)dev;
                    aux.ReadBit(bit);
                }
            }
        }
    }
}
