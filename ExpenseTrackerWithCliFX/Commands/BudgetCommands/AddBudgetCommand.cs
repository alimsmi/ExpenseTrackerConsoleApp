using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using ExpenseTrackerWithCliFX.Common;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Commands.BudgetCommands
{
    [Command("budget add")]
    public class AddBudgetCommand : ICommand
    {
        private readonly JsonBudgetFileHandler _jsonFile;
        public AddBudgetCommand(JsonBudgetFileHandler jsonFile) 
        {
            _jsonFile = jsonFile;
        }

        [CommandOption("year",'y',Description ="For specify the year of budget")]
        public int? Year { get; set; }
        [CommandOption("month",'m',Description ="For Specify the month of budget")]
        public int? Month { get; set; }
        [CommandOption("amount",'a',Description ="Amount of budget",IsRequired =true)]
        public double? Amount { get; set; }
        ValueTask ICommand.ExecuteAsync(IConsole console)
        {
            if (Year != null && IsValidYear((int)Year))
                throw new CommandException("Invalid year insert");
            if (Month != null && IsValidMonth((int)Month))
                throw new CommandException("Invalid month insert");
            if (Amount < 0)
                throw new CommandException("Invalid amount insert, amount can not be 0");
            if (IsExist((int)Year, (int)Month))
                throw new CommandException($"Budget in month : {Month} and  year : {Year} is already exist");
            int action = ConditionDefinder(Year, Month);
            switch (action)
            {
                case 1:
                    AddBudget(DateTime.Now.Year, DateTime.Now.Month, (double)Amount); break;
                case 2:
                    AddBudget((int)Year, DateTime.Now.Month, (double)Amount); break;
                case 3:
                    AddBudget(DateTime.Now.Year, (int)Month, (double)Amount); break;
                case 4:
                    AddBudget((int)Year, (int)Month, (double)Amount);
                    break;
            }
            AnsiConsole.WriteLine("Budget Added");
            Reset();
            return default;
        }
        //method for make several choose for execute method
        private int ConditionDefinder(int? year, int? month)
        {
            if (year == null && month == null)
                return 1;//all options are null
            else if (year != null && month == null)
                return 2;//with year only
            else if (year == null && month != null)
                return 3;//with month only
            else if (year != null && month != null)
                return 4;//with both
            return 0;
        }
        private void AddBudget(int year, int month, double amount)
        {
            if (_jsonFile.IsExist(year, month))
                throw new CommandException("Can not add a budget for one month");
            _jsonFile.AddBudget(amount, month, year);
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
            this.Year = null;
            this.Month = null;
            this.Amount = null;
        }
        private bool IsExist(int year, int month) 
        {
            var budgets = _jsonFile.LoadBudget().FirstOrDefault(e => e.Month == month && e.Year == year);
            if(budgets==null)
                return false;
            return true;
        }
    }
}
