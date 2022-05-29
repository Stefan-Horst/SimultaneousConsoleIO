using System;
using System.Collections.Generic;
using SimultaneousConsoleIO;

namespace Example
{
    class OutputWriter : IOutputWriter //maybe let everybody call this class and only this then calls outputtextwriter? or just combine both output classes (modularity?)?
    {
        public Queue<string> OutputTextQueue = new Queue<string>();

        /*private int sleepTime = 25;//1000 * 5; // 5 seconds
        //private ReminderManager reminderManager;
        private string inputCache;
        private int cursorYInit;*/

        public OutputWriter(/*ReminderManager reminderManager*/)
        {
            //this.reminderManager = reminderManager;

            //Task.Run(() => Run()); //start loop here or in program?
        }

        /*public void Run()
        {
            while (true)
            {
                if (OutputTextQueue.Count > 0)
                {
                    int tempPosY = Console.CursorTop;
                    Console.CursorTop = cursorYInit;
                    for (int i = cursorYInit; i < tempPosY; i++)
                    {
                        Console.WriteLine(new string(' ', Console.BufferWidth));
                    }
                    Console.CursorTop = tempPosY;

                    while (OutputTextQueue.Count > 0)
                        Console.WriteLine(OutputTextQueue.Dequeue());

                    Console.Write(inputCache);
                }

                if (sleepTime > 0)
                    Thread.Sleep(sleepTime);
            }
        }*/

        /*public void UpdateTempData(string inputCache, int cursorYInit)
        {
            this.inputCache = inputCache;
            this.cursorYInit = cursorYInit;
        }*/

        public void AddText(string text) 
        { 
            OutputTextQueue.Enqueue(text); 
        }

        /*public void WriteText()
        {
            while (OutputTextQueue.Count > 0)
                Console.WriteLine(OutputTextQueue.Dequeue());
        }*/

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
