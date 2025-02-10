using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using ExpenseTrackerWithCliFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ExpenseTrackerWithCliFX.Commands.BudgetCommands
{
    [Command("budget update")]
    public class UpdateBudgetCommand : ICommand
    {
        private readonly JsonBudgetFileHandler _fileHandler;
        public UpdateBudgetCommand(JsonBudgetFileHandler fileHanlder) 
        {
            _fileHandler = fileHanlder;
        }
        [CommandOption("where",'i',Description ="For Specify the ID of budget for update")]
        public int? Id { get; set; }
        [CommandOption("year", 'y', Description = "For specify the year of budget")]
        public int? Year { get; set; }
        [CommandOption("month", 'm', Description = "For Specify the month of budget")]
        public int? Month { get; set; }
        [CommandOption("amount", 'a', Description = "Amount of budget")]
        public double? Amount { get; set; }
        public ValueTask ExecuteAsync(IConsole console)
        {
            if (Year != null && !IsValidYear((int)Year))
                throw new CommandException("Invalid year insert");
            if (Month != null && !IsValidMonth((int)Month))
                throw new CommandException("Invalid month insert");
            if (Amount < 0)
                throw new CommandException("Invalid amount insert, amount can not be 0");
            var exsitedBudget = _fileHandler.GetBudgetById((int)Id);
            var budgetToUpdate = new Budget();
            if (Year == null)
                budgetToUpdate.Year = exsitedBudget.Year;
            else
                budgetToUpdate.Year = (int)Year;
            if (Month == null)
                budgetToUpdate.Month = exsitedBudget.Month;
            else
                budgetToUpdate.Month = (int)Month;
            if (Amount == null)
                budgetToUpdate.Amount = exsitedBudget.Amount;
            else
                budgetToUpdate.Amount = (double)Amount;
            budgetToUpdate.Id = (int)Id;
            if (_fileHandler.IsVliadToUpdate(budgetToUpdate.Year, budgetToUpdate.Month, budgetToUpdate.Id))
                throw new CommandException("Can not add a budget for one monthS");
            _fileHandler.UpdateBudget(budgetToUpdate);
            console.Output.Write("Update buget with ID : " + budgetToUpdate.Id + " complete successfully");
            Reset();
            return default;
        }
        private bool IsValidMonth(int month)
        {
            return month > 12 && month < 1;
        }
        private bool IsValidYear(int year)
        {
            int minYear = 1950;
            int maxYear = DateTime.Now.Year + 1;
            return year >= minYear && year < maxYear;
        }
        
        private void Reset()
        {
            this.Id = null;
            this.Year = null;
            this.Month = null;
            this.Amount = null;
        }
    }
}
