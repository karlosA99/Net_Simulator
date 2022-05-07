using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network_Simulator.Instructions;

namespace Network_Simulator
{
    public class Simulator
    {
        private Dictionary<string, Device> devices;
        private List<IConnector> connectors;
        public static int signal_time;
        private bool finished;

        public static int Time { get; set; }
        public string Path { get; set; }
        internal Queue<Instruction> Instructions { get; set; }

        public Simulator()
        {
            signal_time = 2;
            finished = false;
            devices = new Dictionary<string, Device>();
            connectors = new List<IConnector>();
        }

        public void Run_Simulation()
        {
            Instructions = Build_Instructions(Helper.Read_File(Path));
            Instruction current = Instructions.Dequeue();
            int i_time = signal_time;
            while (!finished)
            {
                if(current.Exec_Time <= Time)
                {
                    current.Exec(devices, connectors);
                    //if(current is SendI)
                    //{
                    //    Time--;
                    //}
                    if (Instructions.Count > 0)
                    {
                        current = Instructions.Dequeue();
                    }
                    else
                    {
                        finished = true;
                    }
                }
                Time++;
            }
        }
        public static Queue<Instruction> Build_Instructions(List<string> lines)
        {
            List<Instruction> q = new List<Instruction>();
            Instruction i = null;
            foreach (string item in lines)
            {
                string[] temp = item.Split();
                int time = int.Parse(temp[0]);
                string[] aux = new string[temp.Length - 2];
                Array.Copy(temp, 2, aux, 0, aux.Length);
                switch (temp[1])
                {
                    case "connect":
                        i = new ConnectI(time, aux);
                        break;
                    case "create":
                        aux = new string[temp.Length - 3];
                        Array.Copy(temp, 3, aux, 0, aux.Length);

                        if (temp[2] == "hub")
                            i = new CreateHubI(time, aux);

                        else if (temp[2] == "host")
                            i = new CreateHostI(time, aux);

                        break;
                    case "disconnect":
                        i = new DisconnectI(time, aux);
                        break;
                    case "send":
                        i = new SendI(time, aux);
                        break;
                    default:
                        break;
                }
                q.Add(i);
            }
            q = q.OrderBy(p => p.Exec_Time).ToList();
            return new Queue<Instruction>(q);
        }
    }
}
