using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Physical_Layer.Interfaces;

namespace Physical_Layer
{

    public class Host : Device, ISenderDevice
    {
        public override event DataSent OnDataSent;

        private Port p;
        private PhysicalL_Sender physicalL_Sender;
        public Data dataReceived;
        private bool newData;
        private int timeToRead;

        public Host(string name, int current_time)
        {
            Clock = current_time;
            Name = name;
            p = new LAN_Port(name + "_1");
            Ports = new List<Port>(1);
            p.OnDataReceived += new DataReceived(ReadData);
            p.OnCollitionDetected += new CollitionDetected(RestartReading);
            Ports.Add(p);
            physicalL_Sender = new PhysicalL_Sender(p, Clock);
            timeToRead = int.MaxValue;
        }


        public override void ReadData(Data data, Port p)
        {
            if (data != null)
            {
                if (dataReceived == null || (dataReceived != null && !dataReceived.Equals(data)))
                {
                    dataReceived = data;
                    newData = true;
                    timeToRead = physicalL_Sender.Signal_Time - 1;
                }
            }
        }
        public virtual bool SaveData()
        {
            if (timeToRead == 0)
            {
                if (dataReceived != null && newData)
                {
                    PhysicalL_Writer.Write_File(Clock, Name, Ports[0].Name, "receive", dataReceived.Voltage, false);
                    newData = false;
                    timeToRead = int.MaxValue;
                    return true;
                }
                timeToRead = int.MaxValue;
                return false;
            }
            else
            {
                timeToRead--;
                return false;
            }
        }

        public void SendDatas(List<Data> data)
        {
            physicalL_Sender.AddPackage(data);
            //if(data_sending==null || !data_sending.Equals(data))
            //{
            //    data_sending = data;
            //    PhysicalL_Writer.Write_File(Clock, Name, p.Name, "send", data.Voltage, false);
            //    p.Put_Bit_In_Port(data);
            //}
            //OnDataSent();
            //aqui comprobar si hubo collision haciendo XOR con el cable
        }

        public void Update()
        {
            physicalL_Sender.Update();
        }

        public bool IsActive()
        {
            return physicalL_Sender.IsActive();
        }
        public override void ClockTick()
        {
            base.ClockTick();
            physicalL_Sender.current_time++;
        }

        private void RestartReading(Port port)
        {
            timeToRead = int.MaxValue;
            if (dataReceived != null)
            {
                PhysicalL_Writer.Write_File(Clock, Name, Ports[0].Name, "receive", dataReceived.Voltage, true);
            }
            dataReceived = null;
        }
    }
}
