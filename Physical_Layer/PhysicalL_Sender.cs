using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Configuration;

namespace Physical_Layer
{
    /// <summary>
    /// Constituye el encargado del envio de datos a nivel de capa fisica  y el manejo de colisiones
    /// </summary>
    public class PhysicalL_Sender
    {
        /// <summary>
        /// Puerto por el que se envian los datos
        /// </summary>
        public Port Port;
        /// <summary>
        /// Cola de paquetes de datos que se deben enviar
        /// </summary>
        private Queue<List<Data>> datas;
        /// <summary>
        /// Dato que se esta enviando
        /// </summary>
        private Data data_sending;
        /// <summary>
        /// Maximo tiempo de castigo durante una colision
        /// </summary>
        private int max_penalty_time;
        /// <summary>
        /// Tiempo que se espera para intentar enviar nuevamente un dato tras una colision
        /// </summary>
        private int penalty_time;
        /// <summary>
        /// paquete de datos que se esta enviando
        /// </summary>
        private List<Data> current_package;
        /// <summary>
        /// Indice en el paquete del dato que se esta enviando
        /// </summary>
        private int package_index;
        /// <summary>
        /// True si este dispositivo esta enviando un paquete, false en caso contrario
        /// </summary>
        private bool sending;
        /// <summary>
        /// Tiempo de la simulacion
        /// </summary>
        public int current_time;
        /// <summary>
        /// Tiempo en el que se ha estado enviando un dato
        /// </summary>
        int sending_time;

        /// <summary>
        /// True si este dispositivo esta enviando un paquete, false en caso contrario
        /// </summary>
        public bool Sending { get { return sending; } private set { sending = value; } }
        /// <summary>
        /// Tiempo  que debe durar la transmision de un dato
        /// </summary>
        public int Signal_Time { get; set; }

        public PhysicalL_Sender(Port port, int time)
        {
            this.Port = port;
            port.OnCollitionDetected += new CollitionDetected(CollitionReceive);
            datas = new Queue<List<Data>>();
            Signal_Time = int.Parse(ConfigurationManager.AppSettings.Get("signal_time"));
            current_package = new List<Data>();
            package_index = 0;
            sending = false;
            max_penalty_time = 0;
            penalty_time = 0;
            sending_time = 0;
            current_time = time;
        }
        /// <summary>
        /// Agrega el paquete a la cola de paquetes que se enviaran
        /// </summary>
        public void AddPackage(List<Data> new_data)
        {
            datas.Enqueue(new_data);
        }
        /// <summary>
        /// Actualiza el estado del dispositivo
        /// </summary>
        public void Update()
        {
            if (Port.Connector == null)
            {
                return;
            }

            LoadPackage();
            if (penalty_time > 0)
            {
                penalty_time--;
                return;
            }
            else
            {
                if (current_package.Count > 0)
                {
                    sending = true;
                    data_sending = current_package[package_index];

                    if (!Port.Connector.InCollition)
                    {
                        sending_time++;
                    }
                    //
                    //Si es el primer instante de transmision del dato se envia
                    //                          *
                    //                      si es ultimo
                    //                          *
                    //Como a ese punto solo se llega si no hubo collision entonces
                    //            se toma el envio como un exito
                    //                          
                    if (sending_time == 1)
                    {
                        Port.Put_Bit_In_Port(data_sending);
                    }

                    if (sending_time == Signal_Time)
                    {
                        string device_name = Port.Name.Split('_')[0];
                        PhysicalL_Writer.Write_File(current_time, device_name, Port.Name, "send", data_sending.Voltage, false);
                        Port.Put_Bit_In_Port(null);
                        package_index++;
                        if (package_index == current_package.Count)
                        {
                            current_package.Clear();
                        }
                        sending_time = 0;
                    }

                }
            }
        }
        /// <summary>
        /// Actualiza el estado del paquete que se debe enviar
        /// </summary>
        private void LoadPackage()
        {
            if (current_package.Count == 0)
            {
                if (datas.Count != 0)
                {
                    current_package = datas.Dequeue();
                    package_index = 0;
                    sending = true;
                    max_penalty_time = 16;
                    penalty_time = 0;
                }
                else if (sending)
                {
                    data_sending = null;
                    sending = false;
                }
            }
        }
        /// <summary>
        /// El dispositivo recive el mensaje de colision y segun su estado reinicia algunos valores
        /// </summary>
        public void CollitionReceive(Port port)
        {
            if (sending)
            {
                if (sending_time == 1 && package_index == 0)
                {
                    //Como constituye el inicio del envio del paquete de datos
                    //el dispositivo esperara un tiempo random para volver a intentar la transmision
                    string device_name = port.Name.Split('_')[0];
                    PhysicalL_Writer.Write_File(current_time, device_name, port.Name, "send", data_sending.Voltage, true);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    penalty_time = rnd.Next(1, max_penalty_time);
                    max_penalty_time *= 2;
                    package_index = 0;
                    sending = false;
                    sending_time = 0;
                }
                else
                {
                    //Como la colision ocurrio en medio de una transmision
                    //el dispositivo continuara la transmision por donde iba
                    //reenviando el dato que colisiono
                    sending_time = 0;
                }
            }
        }
        /// <summary>
        /// Devuelve true si tiene algun paquete que enviar, false en caso contrario
        /// </summary>
        public bool IsActive()
        {
            return current_package.Count > 0 || datas.Count>0;
        }
    }
}
