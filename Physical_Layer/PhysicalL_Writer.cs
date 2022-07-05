using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Physical_Layer
{
    public class PhysicalL_Writer
    {
        /// <summary>
        /// Escribe los datos en la direccion configurada teniendo en cuenta si hubo colision
        /// </summary>
        /// <param name="time">Instante de tiempo en el que se realiza la accion</param>
        /// <param name="device_name">Dispositivo que realiza la accion</param>
        /// <param name="port_name">Puerto por el que se realiza la accion</param>
        /// <param name="action">Accion que se realiza</param>
        /// <param name="voltage">Dato que interviene en la accion</param>
        /// <param name="collision">True si hubo colision, False en caso contrario</param>
        public static void Write_File(int time, string device_name, string port_name, string action, int voltage, bool collision)
        {
            string collisionSTR = "ok";
            if (collision)
            {
                collisionSTR = "collision";
            }
            string path = "Physical_Layer_Data\\" + device_name + ".txt";
            string info = time.ToString() + " " + port_name + " " + action + " " + voltage.ToString() + " " + collisionSTR;
            Helper.Write_File(path, info);
        }
        /// <summary>
        /// Escribe los datos en la direccion configurada sin tener en cuenta si hubo colision
        /// </summary>
        /// <param name="time">Instante de tiempo en el que se realiza la accion</param>
        /// <param name="device_name">Dispositivo que realiza la accion</param>
        /// <param name="port_name">Puerto por el que se realiza la accion</param>
        /// <param name="action">Accion que se realiza</param>
        /// <param name="voltage">Dato que interviene en la accion</param>
        public static void Write_File(int time, string device_name, string port_name, string action, int voltage)
        {
            string path = "Physical_Layer_Data\\" + device_name + ".txt";
            string info = time.ToString() + " " + port_name + " " + action + " " + voltage.ToString();
            Helper.Write_File(path, info);
        }
    }
}
