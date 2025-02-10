using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using ExpenseTrackerWithCliFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Commands
{
    [Command("export", Description = "To export expenes into a csv file in desktop")]
    public class ExportCommand : ICommand
    {
        private readonly JsonFileHandler _fileHandler;
        public ExportCommand(JsonFileHandler fileHandler) 
        {
            _fileHandler = fileHandler;
        }
        public ValueTask ExecuteAsync(IConsole console)
        {
            CSVFileHandler.Export(_fileHandler.ReadAllExpenses());
            console.Output.WriteLine("Export done successfully ");
            return default;
        }
    }
}

