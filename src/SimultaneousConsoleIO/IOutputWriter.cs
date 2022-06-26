
namespace SimultaneousConsoleIO
{
    public interface IOutputWriter
    {
        public void AddText(string text);

        public string GetText();
    }
}
