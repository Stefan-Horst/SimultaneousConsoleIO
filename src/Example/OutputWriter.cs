using System;
using System.Collections.Generic;
using SimultaneousConsoleIO;

namespace Example
{
    class OutputWriter : IOutputWriter
    {
        // queue is used in case text is added more than once between each cycle of SimulConsoleIO calling GetText
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
                // format text so that every item from the output queue will be printed in a new line (thus emulating the writeline method)
                while (OutputTextQueue.Count > 1)
                    s += OutputTextQueue.Dequeue() + Environment.NewLine;

                s += OutputTextQueue.Dequeue();
            }

            return s;
        }
    }
}
