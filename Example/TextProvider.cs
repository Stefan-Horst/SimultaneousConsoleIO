using System;
using SimultaneousConsoleIO;

namespace Example
{
    class TextProvider : ITextProvider
    {
        private IOutputWriter outputWriter;

        public void SetOutputWriter(IOutputWriter outputWriter)
        {
            this.outputWriter = outputWriter;
        }

        public void CheckForText()
        {
            if (DateTime.Now.Ticks % 120 == 0) 
                outputWriter.AddText("This message appears every few seconds! The time is " + DateTime.Now.ToLongTimeString());
        }
    }
}
