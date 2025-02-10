using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using ExpenseTrackerWithCliFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Commands.BudgetCommands
{
    [Command("budget delete")]
    public class DeleteBudgetCommand : ICommand
    {
        private readonly JsonBudgetFileHandler _fileHandler;
        [CommandOption("where",'i',Description ="Specify the id of budget to delete",IsRequired =true)]
        public int? Id { get; set; }
        public DeleteBudgetCommand(JsonBudgetFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _fileHandler.DeleteBudget((int)Id);
            console.Output.Write("Budget with ID " + Id + " removed");
            Reset();
            return default;     
        }
        private void Reset() 
        {
            this.Id = null;
        }
    }
}
