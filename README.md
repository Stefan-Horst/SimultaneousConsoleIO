# SimultaneousConsoleIO

***SimultaneousConsoleIO* is a C# tool that allows you to read from and write to the console at the same time.**  
Therefore no more annoying blocking by `Console.ReadLine()` preventing text from being written to the console while the program waits for user input.  

## Features

SimultaneousConsoleIO tries to emulate console functionality from `Console.ReadLine()` as completely as possible and has some additional features such as printing text in the input area which the user can then edit.  
Still, some features are missing as of now. See below for a full list of supported actions.

<details><summary>Keys & Shortcuts</summary>

### Supported Keys & Shortcuts:

| Shortcut | Description |
| --- | --- |
| `Enter` | Execute command |
| `Backspace` | Delete character left of cursor, move cursor one to the left |
| `Arrow left` | Move cursor one to the left |
| `Arrow right` | Move cursor one to the right |
| `Home` | Move cursor to start of input |
| `End` | Move cursor to end of input |
| `ctrl`+`alt`+`...` / `altgr`+`...` | Type certain special characters |
| `ctrl`+`m` | Switch to highlight mode |
| `ctrl`+`a` | Highlight all text |
| `ctrl`+`c` | Copy highlighted text to clipboard |
| `ctrl`+`v` | Paste text from clipboard |
| `ctrl`+`f` | Find keyword |
| `shift`+`Arr left` | Highlight cursor plus previous character |
| `shift`+`Arr right` | Highlight cursor plus next character |
| `shift`+`Arr up` | Highlight text between cursor and same horizontal cursor position one line higher |
| `shift`+`Arr down` | Highlight text between cursor and same horizontal cursor position one line lower |
| `shift`+`Home` | Highlight input before cursor |
| `shift`+`End` | Highlight input after cursor |
| `ctrl`+`shift`+`Home` | Highlight everything before cursor |
| `ctrl`+`shift`+`End` | Highlight everything after cursor |
| `ctrl`+`Arr up` | Scroll one line up |
| `ctrl`+`Arr down` | Scroll one line down |
| `F11` / `Alt`+`Enter` | Maximize console window |

### Unsupported Keys & Shortcuts:

| Shortcut | Description |
| --- | --- |
| `Tab` | Autocompletes file names |
| `Insert` | Switches to insertion mode |
| `ctrl`+`x` | Cut highlighted text to clipboard |
| `Arr right` at end of input | Writes last command char by char |
| `ctrl`+`Backspace` | Delete complete word |
| `ctrl`+`Arr left` | Move cursor to last word |
| `ctrl`+`Arr right` | Move cursor to next word |

### Problematic (and therefore disabled) Keys & Shortcuts:

| Shortcut | Description |
| --- | --- |
| `F...` | All F key functionality except F11 (see above) |
| `ctrl`+`Space` | Does nothing particular in console, but caused problem here |

(list of keys and shortcuts is probably not complete)

</details>

## How to use

SimultaneousConsoleIO provides two interfaces for you to use plus the SimulConsoleIO class which handles all the console logic.
You probably do not need to change that class.  
Below are descriptions of its parts to help you use SimultaneousConsoleIO. You can also take a look at the example implementation located in /src/Example and run it to see SimultaneousConsoleIO in action.

### SimulConsoleIO

The SimulConsoleIO class handles most of the logic and emulates normal console behaviour and functionality (like `Console.ReadLine()`).

The class has the two most important methods:
- `WriteLine`: used to replace `Console.WriteLine()`, allowing text to be written to the console while the ReadLine method is active.
- `ReadLine`: used to replace `Console.ReadLine()`, reading user input while still allowing the console to print output with the WriteLine method. The inputText parameter can be used to write text in the input are, which the user can then edit or complete.

**Important**: do not use `Console.WriteLine()` or `Console.ReadLine()` while using SimulConsoleIO. Doing so might break the console.

### IOutputWriter

The OutputWriter interface is used as the general intermediary to the actual console output (`Console.WriteLine()`) which is and should only be used by SimulConsoleIO.  
Use it (or rather SimulConsoleIO's wrapper method `WriteLine`) to write any text to the console.

The interface has two methods:  
- `AddText`: used to add text to the output (queue), also used by SimulConsoleIO wrapped in a writeline emulating method which should be used instead of `Console.WriteLine()`.
- `GetText`: used by SimulConsoleIO to get all the text to be printed from the OutputWriter and print it. You have to format the text as SimulConsoleIO just prints it as it receives it.

### ITextProvider

The TextProvider interface is used for printing text while the user is entering input.  
As it is independet from the user's actions you can use it to print information from certain events (such as log messages) or wait until certain conditions are true where you want to print text.  

The interface has two methods:  
- `SetOutputWriter`: used to set OutputWriter of TextProvider, therefore making sure TextProvider has an OutputWriter and automize setting it by letting SimulConsoleIO do it.
- `CheckForText`: used by SimulConsoleIO every cycle when output can be printed. Can be used for continually checking conditions and returning some text for printing once the conditions are met.
