using System;
using System.Text;
using System.Threading;

namespace SimultaneousConsoleIO
{
    public class SimulConsoleIO
    {
        private string promptDefault;
        // pause time in main loop (waiting for key input or text output)
        private int sleepTime = 25; // pause as short as possible without eating cpu
        private IOutputWriter outputWriter;
        private ITextProvider textProvider;

        public SimulConsoleIO(IOutputWriter outputWriter, ITextProvider textProvider, string promptDefault)
        {
            this.outputWriter = outputWriter;
            this.textProvider = textProvider;
            this.promptDefault = promptDefault;

            textProvider.SetOutputWriter(outputWriter);
        }

        public SimulConsoleIO(IOutputWriter outputWriter, ITextProvider textProvider) 
            : this(outputWriter, textProvider, "")
        { }

        public string ReadLine()
        {
            return ReadLine(promptDefault);
        }

        public string ReadLine(string prompt)
        {
            StringBuilder cmdInput = new StringBuilder();

            int cursorYInit = Console.CursorTop;
            int cursorXTotal = 0; // like cursorleft but does not reset at new lines
            int cursorXOffset = prompt.Length; // length of prompt before input
            
            Console.Write(prompt);

            ConsoleKeyInfo cki = default;

            do // while (cki.Key != ConsoleKey.Enter)
            {
                if (Console.KeyAvailable)
                { 
                    cki = Console.ReadKey(true);

                    // ctrl key not pressed or alt key pressed (making ctrl+alt possible which equals altgr key), prevents shortcuts like ctrl+i, but allows ones like altgr+q for @
                    if (cki.Key != ConsoleKey.Enter && ((cki.Modifiers & ConsoleModifiers.Control) == 0 || (cki.Modifiers & ConsoleModifiers.Alt) != 0)) 
                    {
                        Console.Write(cki.KeyChar);

                        if (cki.Key == ConsoleKey.Backspace)
                        {
                            Console.CursorLeft++; // counteracts console standard behaviour of moving cursor one to the left

                            // nested if so that backspace does not get added to cmdinput in else-part of statement
                            if (Console.CursorLeft > cursorXOffset || Console.CursorTop > cursorYInit)
                            {
                                bool lineFlag = (cursorXOffset + cursorXTotal) % Console.BufferWidth == 0; // signals when cursor is at first pos of line

                                // only go to line above when current line is empty (otherwise would already happen when first char in line is deleted, because cursor then is at start of line)
                                if (Console.CursorLeft == 1 && Console.CursorTop > cursorYInit && (cmdInput.Length + cursorXOffset) % Console.BufferWidth == 0)
                                {
                                    Console.CursorTop--;
                                    Console.CursorLeft = Console.BufferWidth - 1;
                                    Console.Write(" \b");
                                    Console.CursorLeft++;
                                }
                                else if (cursorXTotal != 0)
                                {
                                    Console.CursorLeft--;
                                    Console.Write(" \b");
                                }
                                else // makes it possible to backspace to start of line (cursor x = 0)
                                { 
                                    Console.CursorLeft--;
                                }

                                if (cursorXTotal > 0) 
                                {
                                    cmdInput.Remove(cursorXTotal - 1, 1);
                                    cursorXTotal--;

                                    // move text after backspace one to the left if there is text after cursor
                                    if (cursorXTotal < cmdInput.Length)
                                    {
                                        int tempPosY = Console.CursorTop;

                                        if (lineFlag == true) // if cursor x at start of line
                                        {
                                            Console.CursorTop = tempPosY - 1;
                                            Console.CursorLeft = Console.BufferWidth - 1;
                                        }
                                        Console.Write(cmdInput.ToString(cursorXTotal, cmdInput.Length - cursorXTotal) + " \b");

                                        Console.CursorTop = cursorYInit + (cursorXTotal + cursorXOffset) / Console.BufferWidth; // '/' discards remainder
                                        Console.CursorLeft = (cursorXTotal + cursorXOffset) % Console.BufferWidth;
                                    }
                                }
                            }
                        }
                        else if (cki.Key == ConsoleKey.LeftArrow)
                        {
                            if (Console.CursorLeft == 0 && Console.CursorTop > cursorYInit) // if cursor x at start of line but not start of input
                            {
                                Console.CursorTop--;
                                Console.CursorLeft = Console.BufferWidth - 1;
                                cursorXTotal--;
                            }
                            else if (Console.CursorLeft > cursorXOffset || Console.CursorTop > cursorYInit) // if cursor x not at start of input
                            {
                                Console.CursorLeft--;
                                cursorXTotal--;
                            }
                        }
                        else if (cki.Key == ConsoleKey.RightArrow)
                        {
                            // nested if so that right arrow does not get added to cmdinput in else-part
                            if (Console.CursorLeft - cursorXOffset < cmdInput.Length - (Console.CursorTop - cursorYInit) * Console.BufferWidth) // cursor can not exeed length of input
                            {
                                if (Console.CursorLeft == Console.BufferWidth - 1) // if cursor x at end of line
                                {
                                    Console.CursorTop++;
                                    Console.CursorLeft = 0;
                                }
                                else
                                {
                                    Console.CursorLeft++;
                                }
                                cursorXTotal++;
                            }
                        }
                        else if (cki.Key == ConsoleKey.UpArrow) //up and down for history
                        {

                        }
                        else if (cki.Key == ConsoleKey.DownArrow)
                        {

                        }
                        else if (cki.Key == ConsoleKey.PageUp) //page up/down for first/last history entry
                        {

                        }
                        else if (cki.Key == ConsoleKey.PageDown)
                        {

                        }
                        else if (cki.Key == ConsoleKey.End) //ende key
                        {
                            Console.CursorTop = cursorYInit + (cursorXOffset + cmdInput.Length) / Console.BufferWidth;
                            Console.CursorLeft = (cursorXOffset + cmdInput.Length) % Console.BufferWidth;

                            cursorXTotal = cmdInput.Length;
                        }
                        else if (cki.Key == ConsoleKey.Home) //pos1 key
                        {
                            Console.CursorTop = cursorYInit;
                            Console.CursorLeft = cursorXOffset;

                            cursorXTotal = 0;
                        }
                        else if (cki.Key == ConsoleKey.Delete) //entf key
                        {
                            // nested if so that del key does not get added to cmdinput in else-part
                            if (Console.CursorLeft - cursorXOffset < cmdInput.Length - (Console.CursorTop - cursorYInit) * Console.BufferWidth) // if not at end of input
                            {
                                cmdInput.Remove(cursorXTotal, 1);

                                if (Console.CursorLeft == Console.BufferWidth - 1) // if cursor x at end of line
                                {
                                    Console.Write(cmdInput.ToString(cursorXTotal, cmdInput.Length - cursorXTotal) + " \b");
                                }
                                else
                                {
                                    Console.Write(" \b" + cmdInput.ToString(cursorXTotal, cmdInput.Length - cursorXTotal) + " \b");
                                }
                                Console.CursorTop = cursorYInit + (cursorXOffset + cursorXTotal) / Console.BufferWidth;
                                Console.CursorLeft = (cursorXOffset + cursorXTotal) % Console.BufferWidth;
                            }
                        }
                        else if (cki.Key == ConsoleKey.Escape)
                        {
                            //add cmd to history before?
                            Console.CursorTop = cursorYInit;
                            Console.CursorLeft = cursorXOffset;

                            Console.Write("0"); // control char for esc to "eat"
                            Console.Write("\b" + new string(' ', cmdInput.Length + 1)); // clear area of input

                            Console.CursorTop = cursorYInit;
                            Console.CursorLeft = cursorXOffset;

                            cmdInput.Clear();
                            cursorXTotal = 0;
                        }
                        else if (cki.Key == ConsoleKey.Tab)
                        {
                            //just ignore for now / prevent user tabbing
                        }
                        else if (cki.Key == ConsoleKey.Insert)
                        {
                            //just ignore for now / mode switching not really necessary
                        }
                        else // handle normal key input
                        {
                            if (cursorXTotal >= cmdInput.Length) // if cursor is at end of input
                            {
                                cmdInput.Append(cki.KeyChar);

                                if ((cursorXOffset + cursorXTotal) % Console.BufferWidth == Console.BufferWidth - 1) // if cursor x at end of line
                                {
                                    Console.CursorTop++;
                                    Console.CursorLeft = 0;
                                }
                            }
                            else
                            {
                                cmdInput.Insert(cursorXTotal, cki.KeyChar);

                                int tempPosY = Console.CursorTop;

                                Console.Write(cmdInput.ToString(cursorXTotal + 1, cmdInput.Length - cursorXTotal - 1)); // move text after insertion one to the right

                                if ((cursorXOffset + cursorXTotal) % Console.BufferWidth == Console.BufferWidth - 1) // if cursor x at end of line
                                {
                                    Console.CursorTop = tempPosY + 1;
                                    Console.CursorLeft = 0;
                                }
                                else
                                {
                                    Console.CursorTop = tempPosY;
                                    Console.CursorLeft = (cursorXOffset + cursorXTotal) % Console.BufferWidth + 1;
                                }
                            }
                            cursorXTotal++;
                        }
                    }
                }
                textProvider.CheckForText();
                PrintText(cmdInput.ToString(), cursorYInit, prompt, cursorXOffset, cursorXTotal); // write text to console "while" getting user input

                cursorYInit = Console.CursorTop - (cursorXOffset + cursorXTotal) / Console.BufferWidth; // changes value relative to changes to cursortop caused by cmd window resizing

                Thread.Sleep(sleepTime);
            }
            while (cki.Key != ConsoleKey.Enter);

            cursorYInit = Console.CursorTop - (cursorXOffset + cursorXTotal) / Console.BufferWidth; // changes value relative to changes to cursortop caused by cmd window resizing

            Console.WriteLine();

            Console.CursorTop = cursorYInit + (cursorXOffset + cmdInput.Length) / Console.BufferWidth; // set cursor y to next line after last (full) line of input
            if ((cursorXOffset + cmdInput.Length) % Console.BufferWidth > 0) // move cursor y one more down if there is one last not full line of input
                Console.CursorTop++;

            string input = cmdInput.ToString();

            Console.WriteLine(input); //del later

            return input;
        }

        // writes all output cached in the outputwriter to the console, returns true if any text was printed, otherwise returns false
        private void PrintText(string inputCache, int cursorYInit, string prompt, int cursorXOffset, int cursorXTotal)
        {
            string output = outputWriter.GetText();
            
            if (output.Length > 0)
            {
                // set to cursor y pos last line of input
                int tempPosY = cursorYInit + (cursorXOffset + inputCache.Length) / Console.BufferWidth;
                if ((cursorXOffset + inputCache.Length) % Console.BufferWidth > 0)
                    tempPosY++;

                Console.CursorTop = cursorYInit;
                Console.CursorLeft = 0;
                for (int i = cursorYInit; i <= tempPosY; i++) // clear current user input
                {
                    Console.WriteLine(new string(' ', Console.BufferWidth));
                }
                Console.CursorTop = cursorYInit;

                Console.WriteLine(output);
                
                tempPosY = Console.CursorTop;
                int tempPosX = Console.CursorLeft;

                Console.Write(prompt + inputCache);

                // make cursor break line when input reaches end of line
                /*if (prompt.Length + inputCache.Length > 0 && (prompt.Length + inputCache.Length) % Console.BufferWidth == 0)
                {
                    Console.CursorTop++;
                    Console.CursorLeft = 0;
                }*/
                //else //does prompt cause problems with window resizing? if yes change to cursorxtotal + prompt.length above
                {
                    Console.CursorTop = tempPosY + (cursorXTotal + cursorXOffset) / Console.BufferWidth; // '/' discards remainder
                    Console.CursorLeft = tempPosX + (cursorXTotal + cursorXOffset) % Console.BufferWidth;
                }
            }
        }

        public void WriteLine(string text)
        {
            outputWriter.AddText(text);
        }
    }
}
