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
    [Command("list",Description ="Command To Show List of expenses")]
    public class ListCommand : ICommand
    {
        private readonly JsonFileHandler _fileHandler;
        public ListCommand(JsonFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }
        [CommandOption("category", 'c', Description = "Select expenses List by categories => Jaunt,HouseHole,Educational,Medicine", IsRequired = false)]
        public string? Category { get; set; }
        [CommandOption("year", 'y', Description = "See the year's expenses")]
        public int? Year { get; set; }
        [CommandOption("month", 'm', Description = "See Specific month's expense ")]
        public int? Month { get; set; } 

        ValueTask ICommand.ExecuteAsync(IConsole console)
        {
            int action = ModeDefinder(Category, Year, Month);
            switch (action) 
            {
                case 1:
                    ShowTable(console); break;
                case 2:
                    ShowTableByCategories(console,Category); break;
                case 3:
                    ShowTableByYearAndCategories(console, Category, (int)Year);break;
                case 4:
                    ShowTableByYaer(console,(int)Year);break;
                case 5:
                    ShowTableByYearAndMonth(console, (int)Year, (int)Month);break;
                case 6:
                    ShowTableByAllCondition(console, Category,(int)Year,(int)Month);break;
                default:
                    throw new CommandException("Something wrong happend");
            }
            Reset();
            return default;
        }
        private void ShowTable(IConsole console)
        {
            var list = _fileHandler.ReadAllExpenses();
            if (!list.Any())
            {
                console.Output.WriteLine("There is no Expense");
                return;
            }
            var table = new Table();
            table.Title = new TableTitle("Expenses");
            table.BorderStyle = new Style(Color.Green4);
            table.RoundedBorder();

            table.AddColumn("ID");
            table.AddColumn("Description");
            table.AddColumn("Amount");
            table.AddColumn("Date");
            table.AddColumn("Category");
            foreach (var expense in list)
            {
                var textes = new List<Text>()
                {
                    new Text(expense.Id.ToString()),
                    new Text(expense.Description),
                    new Text(expense.Amount.ToString()),
                    new Text(expense.Date.ToShortDateString()),
                    new Text(expense.Category.ToString())
                };
                table.AddRow(textes);
            }
            AnsiConsole.Write(table);
        }
        private void ShowTableByCategories(IConsole console, string categoryInString) 
        {
            var category = CategorySelecter(categoryInString);
            var list = _fileHandler.ReadAllExpenses().Where(e=>e.Category == category);
            if (!list.Any())
            {
                console.Output.WriteLine("There is no Expense");
                return;
            }
            var table = new Table();
            table.Title = new TableTitle("Expenses in "+category.ToString());
            table.BorderStyle = new Style(Color.Green4);
            table.RoundedBorder();

            table.AddColumn("ID");
            table.AddColumn("Description");
            table.AddColumn("Amount");
            table.AddColumn("Date");
            foreach (var expense in list)
            {
                var textes = new List<Text>()
                {
                    new Text(expense.Id.ToString()),
                    new Text(expense.Description),
                    new Text(expense.Amount.ToString()),
                    new Text(expense.Date.ToShortDateString())
                };
                table.AddRow(textes);
            }
            AnsiConsole.Write(table);
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
        private void ShowTableByYaer(IConsole console, int year) 
        {
            var list = _fileHandler.ReadAllExpenses().Where(e => e.Date.Year == year);
            if (!list.Any())
            {
                console.Output.WriteLine("There is no Expense");
                return;
            }
            var table = new Table();
            table.Title = new TableTitle("Expenses in "+year);
            table.BorderStyle = new Style(Color.Green4);
            table.RoundedBorder();

            table.AddColumn("ID");
            table.AddColumn("Description");
            table.AddColumn("Amount");
            table.AddColumn("Date");
            table.AddColumn("Category");
            foreach (var expense in list)
            {
                var textes = new List<Text>()
                {
                    new Text(expense.Id.ToString()),
                    new Text(expense.Description),
                    new Text(expense.Amount.ToString()),
                    new Text(expense.Date.ToShortDateString()),
                    new Text(expense.Category.ToString())
                };
                table.AddRow(textes);
            }
            AnsiConsole.Write(table);
        }
        private void ShowTableByYearAndCategories(IConsole console,string categoryInString,int year) 
        {
            var category = CategorySelecter(categoryInString);
            var list = _fileHandler.ReadAllExpenses().Where(e => e.Category == category).Where(e=>e.Date.Year == year);
            if (!list.Any())
            {
                console.Output.WriteLine("There is no Expense");
                return;
            }
            var table = new Table();
            table.Title = new TableTitle("Expenses in " + category.ToString()+" and "+ year);
            table.BorderStyle = new Style(Color.Green4);
            table.RoundedBorder();

            table.AddColumn("ID");
            table.AddColumn("Description");
            table.AddColumn("Amount");
            foreach (var expense in list)
            {
                var textes = new List<Text>()
                {
                    new Text(expense.Id.ToString()),
                    new Text(expense.Description),
                    new Text(expense.Amount.ToString()),
                };
                table.AddRow(textes);
            }
            AnsiConsole.Write(table);
        }
        private void ShowTableByYearAndMonth(IConsole console,int year,int month) 
        {
            var list = _fileHandler.ReadAllExpenses().Where(e=>e.Date.Year==year&&e.Date.Month==month);
            if (!list.Any())
            {
                console.Output.WriteLine("There is no Expense");
                return;
            }
            var table = new Table();
            table.Title = new TableTitle("Expenses in Year : "+year+" Month : "+month);
            table.BorderStyle = new Style(Color.Green4);
            table.RoundedBorder();

            table.AddColumn("ID");
            table.AddColumn("Description");
            table.AddColumn("Amount");
            table.AddColumn("Category");
            foreach (var expense in list)
            {
                var textes = new List<Text>()
                {
                    new Text(expense.Id.ToString()),
                    new Text(expense.Description),
                    new Text(expense.Amount.ToString()),
                    new Text(expense.Category.ToString())
                };
                table.AddRow(textes);
            }
            AnsiConsole.Write(table);
        }
        private void ShowTableByAllCondition(IConsole console,string category,int year,int month) 
        {
            var realCategory = CategorySelecter(category);
            var list = _fileHandler.ReadAllExpenses().Where(e=>e.Category==realCategory&&e.Date.Year==year&&e.Date.Month==month);
            if (!list.Any())
            {
                console.Output.WriteLine("There is no Expense");
                return;
            }
            var table = new Table();
            table.Title = new TableTitle("Expenses in Category : "+category+" Year : "+year+" month : "+month);
            table.BorderStyle = new Style(Color.Green4);
            table.RoundedBorder();

            table.AddColumn("ID");
            table.AddColumn("Description");
            table.AddColumn("Amount");
            table.AddColumn("Category");
            foreach (var expense in list)
            {
                var textes = new List<Text>()
                {
                    new Text(expense.Id.ToString()),
                    new Text(expense.Description),
                    new Text(expense.Amount.ToString()),
                    new Text(expense.Category.ToString())
                };
                table.AddRow(textes);
            }
            AnsiConsole.Write(table);
        }
        private bool IsYearValid(int year)
        {
            int minYear = 1950;
            int maxYear = DateTime.Now.Year + 1;//define current year + 1 to expenses may be for future

            return !(year >= minYear && year <= maxYear);
        }
        //method to define what table should generate
        private int ModeDefinder(string categoryInString,int? year,int? month) 
        {
            
            if (Year != null && IsYearValid((int)Year))
                throw new CommandException("Enter incorrect year");
            if (Year == null && Month != null)
                throw new CommandException("Invalid Year insert attempt => specify month with year you entered");
            if (String.IsNullOrEmpty(Category) && Month == null && Year == null)
                return 1;//1 is normal table
            else if (!String.IsNullOrEmpty(Category) && Month == null && Year == null)
                return 2;//2 is with category only
            else if (!String.IsNullOrEmpty(Category) && Month == null && Year != null)
                return 3;//3 is with year and category
            else if (String.IsNullOrEmpty(Category) && Month == null && Year != null)
                return 4;//4 is with year only
            else if (String.IsNullOrEmpty(Category) && Month != null && Year != null)
                return 5;//5 is with year and month
            else if (!String.IsNullOrEmpty(Category) && Month != null && Year != null)
                return 6;//6 is with year and month and category
                return 0;
        }
        private void Reset() 
        {
            this.Year = null;
            this.Month = null;
            this.Category = null;
        }
    }
}
