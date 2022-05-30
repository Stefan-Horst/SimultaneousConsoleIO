﻿using System;
using SimultaneousConsoleIO;

namespace Example
{
    class Program
    {
        static void Main()
        {
            OutputWriter outputWriter = new OutputWriter();
            TextProvider textProvider = new TextProvider();
            SimulConsoleIO simulIO = new SimulConsoleIO(outputWriter, textProvider);

            while (true)
            {
                string input = simulIO.ReadLine("> Your input: ");

                outputWriter.AddText("You just typed: " + input);
            }
        }
    }
}
