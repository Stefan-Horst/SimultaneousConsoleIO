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
            
        }
    }
}
