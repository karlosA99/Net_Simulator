using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;
using Physical_Layer.Interfaces;
using Network_Simulator;

namespace Network_Simulator.Instructions
{
    public class SendI : Instruction
    {
        /// <summary>
        /// Instrucion que ordena a un dispositivo a enviar una serie de bits
        /// </summary>
        /// <param name="time">Instante de tiempo en el que se debe ejecutar la instruccion</param>
        /// <param name="args">Argumentos que recibe la instrucion</param>
        public SendI(int time, string[] args) : base(time, args)
        {
        }

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            if(devices[Args[0]] is Host)
            {
                Host sender = (Host)devices[Args[0]];
                sender.SendDatas(Helper.String_To_DataList(Args[1]));
            }
            else
            {
                throw new Exception("Transmitter is not a host");
            }
        }
    }
}
