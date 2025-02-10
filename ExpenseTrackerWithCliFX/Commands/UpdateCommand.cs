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

namespace ExpenseTrackerWithCliFX.Commands
{
    [Command("update", Description = "Use to update an existed expense")]
    public class UpdateCommand : ICommand
    {
        private readonly JsonFileHandler _fileHandler;
        private readonly JsonBudgetFileHandler _budgetFileHanlder;
        [CommandOption("description",'d',Description="Update description")]
        public string? Description { get; set; }
        [CommandOption("amount",'a',Description="Update amount of expense")]
        public double? Amount { get; set; }
        [CommandOption("date",'t',Description="Update Time of expense yyyy-mm-dd format")]
        public DateOnly? Date { get; set; }
        [CommandOption("category",'c',Description ="Update Categroy of expense = > Educational , Jaunt , HouseHold , Medicine")]
        public string? Category { get; set; }
        [CommandOption("where",'w',Description ="Define Id of expense to update",IsRequired =true)]
        public int? Where { get; set; }
        public UpdateCommand(JsonFileHandler fileHandler,JsonBudgetFileHandler budgetFileHanlder) 
        {
            _budgetFileHanlder = budgetFileHanlder;
            _fileHandler = fileHandler;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            if (Date != null&&!IsDateValid((DateOnly)Date))
                throw new CommandException("Invalid date insert");
            if (Amount < 0)
                throw new CommandException("Invalid amount insert, amount can not be 0 or negetive number");
            var expenseToUpdate = new ExpenseEntity();
            var expense = _fileHandler.GetExpenseById((int)Where);
            if (expense == null)
                throw new CommandException("Null id : "+Where);
            if (Description == null)
            {
                expenseToUpdate.Description = expense.Description;
            }
            else { expenseToUpdate.Description = Description; }
            if (Amount == null)
            {
                expenseToUpdate.Amount = expense.Amount;
            }
            else { expenseToUpdate.Amount = (double)Amount; }
            if (Category == null)
            {
                expenseToUpdate.Category = expense.Category;
            }
            else { expenseToUpdate.Category = CategorySelecter(Category); }
            if (Date == null)
            {
                expenseToUpdate.Date = expense.Date;
            }
            else { expenseToUpdate.Date = (DateOnly)Date; }

            expenseToUpdate.Id = expense.Id;
            _fileHandler.UpdateExpenseByEntity(expenseToUpdate);
            console.Output.WriteLine("Update Expense ID : "+Where+" done successfully");
            if (CheckAmountAndBudget(expenseToUpdate.Date.Month, expenseToUpdate.Date.Year))
            {
                var warningRule = new Rule("[red]Warning[/]");
                AnsiConsole.Write(warningRule);
                AnsiConsole.WriteLine();
                var messageRule = new Rule("[blue]You amount of expense are exceeded of your budget[/]");
                warningRule.RuleStyle("red dim");
                AnsiConsole.Write(messageRule);
            }
            Reset();
            return default;
        }
        private bool CheckAmountAndBudget(int month, int year)
        {
            double budgetAmount = _budgetFileHanlder.GetAmountOfBudgetByMonthAndYear(month, year);
            double monthAmount = GetAmountOfMonth(month, year);
            if (monthAmount > budgetAmount)
                return true;//return true if user exceed the budget
            return false;
        }
        private double GetAmountOfMonth(int month, int year)
        {
            var expenses = _fileHandler.ReadAllExpenses().Where(e => e.Date.Month == month && e.Date.Year == year);
            double amount = 0;
            foreach (var ex in expenses) { amount += ex.Amount; }
            return amount;
        }
        private ExpenseCategory CategorySelecter(string categoryAsString)
        {
            switch (categoryAsString.ToLower())
            {
                case "educational":
                    return ExpenseCategory.Educational;
                case "medicine":
                    return ExpenseCategory.Medicine;
                case "jaunt":
                    return ExpenseCategory.Jaunt;
                case "household":
                    return ExpenseCategory.HouseHold;
            }
            throw new ArgumentNullException(nameof(ExpenseCategory));
        }
        private bool IsDateValid(DateOnly date)
        {
            var minDate = new DateOnly(1950, 1, 1);
            var maxDate = DateOnly.FromDateTime(DateTime.Now);
            return date >= minDate && date <= maxDate;
        }
        private void Reset() 
        {
            this.Where = null;
            this.Amount = null;
            this.Category = null;
            this.Description = null;
            this.Date = null;
        }
    }
}
