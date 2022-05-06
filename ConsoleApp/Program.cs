using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulator;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the read address");
            string path = Console.ReadLine();
            Network_Simulator simulator = new Network_Simulator();
            simulator.Path = path;
            if(simulator.Path == null)
            {
                simulator.Run_Simulation();
            }
        }
    }
}
