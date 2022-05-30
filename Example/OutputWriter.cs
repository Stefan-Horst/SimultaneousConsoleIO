using System;
using System.Collections.Generic;
using SimultaneousConsoleIO;

namespace Example
{
    class OutputWriter : IOutputWriter
    {
        public Queue<string> OutputTextQueue = new Queue<string>();

        public void AddText(string text) 
        { 
            OutputTextQueue.Enqueue(text); 
        }

        public string GetText()
        {
            string s = "";

            if (OutputTextQueue.Count > 0)
            {
                while (OutputTextQueue.Count > 1)
                    s += OutputTextQueue.Dequeue() + Environment.NewLine;

                s += OutputTextQueue.Dequeue();
            }

            return s;
        }
    }
}
