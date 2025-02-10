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
    [Command("budget", Description = "Command for define budget for each month")]
    public class BudgetCommand : ICommand
    {
        private readonly JsonBudgetFileHandler _file;
        public BudgetCommand(JsonBudgetFileHandler file)
        {
            _file = file;
        }
        [CommandOption("month", 'm', Description = "Select month for define budget for that")]
        public int? Month { get; set; }

        [CommandOption("year", 'y', Description = "Select year for see all buget in one year")]
        public int? Year { get; set; }

        [CommandOption("amount", 'a', Description = "Option for insert the amount of budget")]
        public double? Amount { get; set; }

        [CommandOption("where", 'w', Description = "For specify the ID of Budget ( do not use out of delete or update option )")]
        public int? Where { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {

            if (Month != null && IsValidMonth((int)Month))
                throw new CommandException("Invalid Month Insert");
            if (Year != null && !IsValidYear((int)Year))
                throw new CommandException("Invalid Year Insert => min 1950 max current year + 1");
            int action = ConditionDefinder(Year, Month);
            switch (action)
            {
                case 1:
                    ShowTableWithAllDetails(_file.GetBudgets(), "All Budgets"); break;
                case 2:
                    ShowTableWithYear(_file.GetBudgetForYear((int)Year), "Budgets in " + Year); break;
                case 3:
                    ShowTableWithMonth(_file.GetBudgetByMonth((int)Month), "Budgets in " + Month + " month of current Year"); break;
            }
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
        private void ShowTableWithAllDetails(List<Budget> budgets, string tableDescription)
        {
            var table = new Table();
            table.Width(80);
            table.AddColumn("ID");
            table.AddColumn("Year");
            table.AddColumn("Month");
            table.AddColumn("Amount");
            table.Title = new TableTitle(tableDescription);
            table.BorderStyle = new Style(Color.Aquamarine1);
            foreach (var budget in budgets)
            {
                var texts = new List<Text>()
                {
                    new Text(budget.Id.ToString()),
                    new Text(budget.Year.ToString()),
                    new Text(budget.Month.ToString()),
                    new Text(budget.Amount.ToString())
                };
                table.AddRow(texts);
            }
            AnsiConsole.Write(table);

        }
        private void ShowTableWithMonth(List<Budget> budgets, string tableDescription)
        {
            var table = new Table();
            table.Width(80);
            table.AddColumn("ID");
            table.AddColumn("Month");
            table.AddColumn("Amount");
            table.Title = new TableTitle(tableDescription);
            table.BorderStyle = new Style(Color.Aquamarine1);
            foreach (var budget in budgets)
            {
                var texts = new List<Text>()
                {
                    new Text(budget.Id.ToString()),
                    new Text(budget.Month.ToString()),
                    new Text(budget.Amount.ToString())
                };
                table.AddRow(texts);
            }
            AnsiConsole.Write(table);

        }
        private void ShowTableWithYear(List<Budget> budgets, string tableDescription)
        {
            var table = new Table();
            table.Width(80);
            table.AddColumn("ID");
            table.AddColumn("Year");
            table.AddColumn("Amount");
            table.Title = new TableTitle(tableDescription);
            table.BorderStyle = new Style(Color.Aquamarine1);
            foreach (var budget in budgets)
            {
                var texts = new List<Text>()
                {
                    new Text(budget.Id.ToString()),
                    new Text(budget.Year.ToString()),
                    new Text(budget.Amount.ToString())
                };
                table.AddRow(texts);
            }
            AnsiConsole.Write(table);

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
            this.Where = null;
        }
    }
}
