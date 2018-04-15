using System;
using System.Diagnostics;
using System.Globalization;

namespace CyclotronSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            //set cultureInfo to en
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");

            //write to file:
            var writer = System.IO.File.CreateText("SWC2.csv");
            Console.SetOut(writer);

            //maak headers
            Console.WriteLine("Start,Freq,Time,Failed");
            //freq range
            double freq = 14.8;
            for (int i = 0; i < 50; i++)
            {
                double start = 0;
                //insertTime range
                for (int j = 0; j < 10; j++)
                {                   
                    Cyclotron cyclo = new Cyclotron(0.7, 0.0001, start, 10, 1, 1.7, 50, 1, freq, true, true);
                    while (cyclo.T < 10)
                    {
                        cyclo.SimulationStep();
                        if (cyclo.bounced || cyclo.pos.LengthSquared() >= 102400) break;                   
                    }
                    Console.WriteLine(start+","+freq+","+cyclo.T+"," + cyclo.bounced);
                    start += 1 / (freq * 10);
                }
                freq += 0.01;
            }

            //stop writing to file
            writer.Dispose();
        }

    }
}