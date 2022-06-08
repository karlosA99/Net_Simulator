using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network_Simulator.Instructions;
using System.Configuration;
using System.Collections.Specialized;

namespace Network_Simulator
{
    public class Simulator
    {
        private Dictionary<string, Device> devices;
        private List<IConnector> connectors;
        
        private bool finished;

        public static int Time { get; set; }

        public static int Signal_Time = 10;
        public string Path { get; set; }
        internal Queue<Instruction> Instructions { get; set; }

        public Simulator()
        {
            Signal_Time = int.Parse(ConfigurationManager.AppSettings.Get("signal_time"));
            finished = false;
            devices = new Dictionary<string, Device>();
            connectors = new List<IConnector>();
        }

        public void Run_Simulation()
        {
            Instructions = Build_Instructions(Helper.Read_File(Path));
            Instruction current = Instructions.Dequeue();
            while (!finished)
            {
                if(current.Exec_Time <= Time)
                {
                    if (current is CreateI)
                    {
                        ((CreateI)current).OnNewDevice += new NewDevice(SubscribeDev);
                    }
                    current.Exec(devices, connectors);
                    
                    
                    if (Instructions.Count > 0)
                    {
                        current = Instructions.Dequeue();
                    }
                    else
                    {
                        finished = true;
                    }
                }
                else
                {
                    ClockTick();
                }
            }
        }

        private void SubscribeDev(Device dev)
        {
            dev.OnDataSent+= new DataSent(ClockTick);
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

                        else if(temp[2]== "switch")
                            i = new CreateSwitchI(time, aux);

                        break;
                    case "mac":
                        i = new SetMacI(time, aux);
                        break;

                    case "disconnect":
                        i = new DisconnectI(time, aux);
                        break;

                    case "send":
                        i = new SendI(time, aux);
                        break;

                    case "send_frame":
                        i = new SendFrameI(time, aux);
                        break;

                    default:
                        break;
                }
                q.Add(i);
            }
            q = q.OrderBy(p => p.Exec_Time).ToList();
            return new Queue<Instruction>(q);
        }
        public void ClockTick()
        {
            Time++;
            ICollection<string> aux = devices.Keys;
            foreach (string key in aux)
            {
                devices[key].ClockTick();
            }
        }
    }
}
