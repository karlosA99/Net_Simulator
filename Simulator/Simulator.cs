using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network_Simulator.Instructions;
using Physical_Layer.Interfaces;
using System.Configuration;

namespace Network_Simulator
{
    public class Simulator
    {
        private Dictionary<string, Device> devices;
        private List<IConnector> connectors;
        
        private bool finished;

        public static int Time { get; set; }
        public string Path { get; set; }
        public static int Signal_Time { get; private set; }
        internal Queue<Instruction> Instructions { get; set; }

        public Simulator()
        {
            Signal_Time= int.Parse(ConfigurationManager.AppSettings.Get("signal_time"));
            finished = false;
            devices = new Dictionary<string, Device>();
            connectors = new List<IConnector>();
        }

        public void Run_Simulation()
        {
            Instructions = Build_Instructions(Helper.Read_File(Path));
            while (!finished)
            {
                while (Instructions.Count>0 && Instructions.Peek().Exec_Time == Time)
                {
                    Instruction current=Instructions.Dequeue();
                    current.Exec(devices, connectors);
                }
                ClockTick();
            }
        }

        private void ClockTick()
        {
            foreach(Device sender in devices.Values)
            {
                if(sender is ISenderDevice)
                {
                    ((ISenderDevice)sender).Update();
                }
            }

            foreach (Device sender in devices.Values)
            {
                if (sender is ISenderDevice)
                {
                    ((ISenderDevice)sender).SaveData();
                }
            }

            foreach (Device device in devices.Values)
            {
                device.ClockTick();
            }
            foreach(IConnector connector in connectors)
            {
                if(connector.InCollition)
                {
                    connector.CleanConnector();
                }
            }
            finished = Finished();
            Time++;
        }
        private bool Finished()
        {
            if (Instructions.Count > 0)
            {
                return false;
            }
            foreach (Device sender in devices.Values)
            {
                if (sender is ISenderDevice)
                {
                    if (((ISenderDevice)sender).IsActive())
                    {
                        return false;
                    }
                }
            }
            return true;
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

                        else if (temp[2] == "switch")
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
    }
}
