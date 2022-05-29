using SimultaneousConsoleIO;

namespace Example
{
    class TextProvider : ITextProvider
    {
        private IOutputWriter outputWriter;
        //private ReminderManager reminderManager;

        public void SetOutputWriter(IOutputWriter outputWriter)
        {
            this.outputWriter = outputWriter;
        }

        public void CheckForText()
        {
            //reminderManager.GetDueReminders(DateTime.Now).ForEach(i => outputWriter.AddText(i.ToString()));
        }
    }
}
