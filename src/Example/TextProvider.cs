using System;
using SimultaneousConsoleIO;

namespace Example
{
    public class TextProvider : ITextProvider
    {
        private IOutputWriter outputWriter;

        public void SetOutputWriter(IOutputWriter newOutputWriter)
        {
            outputWriter = newOutputWriter;
        }

        public void CheckForText()
        {
            // just add some text to be written to the console every now and then and completely independently from the console input
            // in a real application this could be log messages or other strings to be printed that could occur at any time
            if (DateTime.Now.Ticks % 120 == 0)
                outputWriter.AddText("This message appears every few seconds! The time is " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
        }
    }
}
