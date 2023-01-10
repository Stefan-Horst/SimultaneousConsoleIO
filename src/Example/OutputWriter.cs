using System.Collections.Generic;
using System.Text;
using SimultaneousConsoleIO;

namespace Example
{
    public class OutputWriter : IOutputWriter
    {
        // queue is used in case text is added more than once between each cycle of SimulConsoleIO calling GetText
        private Queue<string> outputTextQueue = new Queue<string>();
        // text which will be at beginning of all output
        private string startText;
        
        public string StartText { get => startText; set => startText = value; }
        
        public OutputWriter(string startText = "")
        {
            this.startText = startText;
        }

        public void AddText(string text) 
        { 
            outputTextQueue.Enqueue(startText + text); 
        }

        public string GetText()
        {
            StringBuilder s = new StringBuilder();
            
            while (outputTextQueue.Count > 0)
                s.Append(outputTextQueue.Dequeue());

            return s.ToString();
        }
    }
}
