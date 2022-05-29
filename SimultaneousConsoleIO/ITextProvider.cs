
namespace SimultaneousConsoleIO
{
    public interface ITextProvider
    {
        public void SetOutputWriter(IOutputWriter outputWriter);

        public void CheckForText();
    }
}
