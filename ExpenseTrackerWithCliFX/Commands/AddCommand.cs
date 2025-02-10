using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using ExpenseTrackerWithCliFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliFx.Exceptions;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Spectre.Console;
using System.ComponentModel;

namespace ExpenseTrackerWithCliFX.Commands
{
    [Command("add",Description ="Command for add expenses")]
    public class AddCommand : ICommand
    {
        [CommandOption("description",'d',Description="Description for your expense",IsRequired =true)]
        public string? Description { get; set; }
        [CommandOption("amount",'a',Description ="Amount of your expense",IsRequired =true)]
        public double? Amount { get; set; }
        [CommandOption("category",'c',Description ="Category of your expense => Educational,Jaunt,Medicine,HouseHold",IsRequired =true)]
        public string? Category { get; set; }
        [CommandOption("date",'t',Description="Define Date of your expese in yyyy-mm-dd format or let plank for today's Date")]
        public string? Date { get; set; }
        private readonly JsonFileHandler _fileHandler;
        private readonly JsonBudgetFileHandler _budgetFile;
        public AddCommand(JsonFileHandler fileHandler,JsonBudgetFileHandler budgetFile)
        {
            _budgetFile = budgetFile;
            _fileHandler = fileHandler;
        }
        //Excute method by add command
        public ValueTask ExecuteAsync(IConsole console)
        {
            if (Amount <= 0)
                throw new CommandException("Amount Can't be 0 or negetive number");
            string description = Description;
            double amount =(double) Amount;
            DateOnly date;
            if (Date == null)
                date = DateOnly.FromDateTime(DateTime.Now);
            else
            {
                date = DateOnly.Parse(Date);
            }
            ExpenseCategory expenseCategory = CategorySelecter(Category);
            ExpenseEntity entity = new ExpenseEntity(description,date,amount,expenseCategory);
            _fileHandler.AddExpense(entity);
            console.Output.WriteLine("Added Expense With ID : "+entity.Id+"");
            if (CheckAmountAndBudget(entity.Date.Month, entity.Date.Year)) 
            {
                var warningRule = new Rule("[red]Warning[/]");
                AnsiConsole.Write(warningRule);
                AnsiConsole.WriteLine();
                var messageRule = new Rule("[blue]Amount's of your expenses are exceeded of your budget[/]");
                warningRule.RuleStyle("red dim");
                AnsiConsole.Write(messageRule);
            }
            Reset();
            return default;
        }
        private bool CheckAmountAndBudget(int month,int year) 
        {
            double budgetAmount = _budgetFile.GetAmountOfBudgetByMonthAndYear(month, year);
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
        private void Reset() 
        {
            this.Description = null;
            this.Amount = null;
            this.Category = null;
            this.Date = null;
        }
    }
}
