using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer;
using Network_Simulator;

namespace Network_Simulator.Instructions
{
    public class SendI : Instruction
    {
        public SendI(int time, string[] args) : base(time, args){}

        public override void Exec(Dictionary<string, Device> devices, List<IConnector> connectors)
        {
            string datas = Args[1];
            Device transmitter = devices[Args[0]];

            for (int i = 0; i < datas.Length; i++)
            {
                //Aqui hay que agregar el for signal_time para enviar signal_time veces la dataz
                Data data = new Data(int.Parse(datas[i].ToString()));
                transmitter.SendData(data, Exec_Time);
            }          
        } 
    }
}
