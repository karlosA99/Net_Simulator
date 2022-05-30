using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network_Simulator;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the read address");
            string path = Console.ReadLine();
            CleanPath();
            Simulator simulator = new Simulator();
            simulator.Path = path;
            if(simulator.Path != null)
            {
                simulator.Run_Simulation();
            }
        }

        private static void CleanPath()
        {
            List<string> strDirectories = Directory.GetFiles("Link_Layer_Data\\", "*", SearchOption.AllDirectories).ToList();
            foreach (string strDirectory in strDirectories)
            {
               File.Delete(strDirectory);
            }

            List<string> strDirectories2 = Directory.GetFiles("Physical_Layer_Data\\","*", SearchOption.AllDirectories).ToList();
            foreach (string strDirectory2 in strDirectories2)
            {
                File.Delete(strDirectory2);
            }
        }
    }
}
