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
using static System.Net.Mime.MediaTypeNames;

namespace ExpenseTrackerWithCliFX.Commands
{
    [Command("summary",Description ="Check Summary of expenses")]
    public class SummaryCommand : ICommand
    {
        private readonly JsonFileHandler _fileHandler;

        [CommandOption("month",'m',Description ="See Summary of Specific month(Current year if dont specify year)")]
        public int? Month { get; set; }

        [CommandOption("year",'y',Description ="option for select the year")]
        public int? Year { get; set; }
        public SummaryCommand(JsonFileHandler fileHandler) 
        {
            _fileHandler = fileHandler;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            if (Year != null && !IsYearValid(Year))
                throw new CommandException("Invalid Year insert");
            if (Month != null && !IsValidMonth(Month))
                throw new CommandException("Invalid Month insert");

            if (Year == null && Month == null)
                ShowAllSummary(console);
            else if (Year == null & Month != null)
                ShowSummaryInMonth(console, (int)Month);
            else if (Year != null && Month != null)
                ShowSummaryInMonthAndYear(console, (int)Year, (int)Month);
            else if (Year != null && Month == null)
                ShowSummaryOfYear(console, (int)Year);
            Reset();
            return default;
        }
        public void ShowSummaryOfYear(IConsole console,int year) 
        {
            var list = _fileHandler.ReadAllExpenses().Where(e =>e.Date.Year == year);
            double educationalSummary = 0;
            double jauntSummary = 0;
            double medicineSummary = 0;
            double houseHoleSummary = 0;
            foreach (var item in list)
            {
                if (item.Category == ExpenseCategory.Educational)
                    educationalSummary += item.Amount;
                else if (item.Category == ExpenseCategory.Jaunt)
                    jauntSummary += item.Amount;
                else if (item.Category == ExpenseCategory.Medicine)
                    medicineSummary += item.Amount;
                else if (item.Category == ExpenseCategory.HouseHold)
                    houseHoleSummary += item.Amount;
            }
            RenderChart(educationalSummary, medicineSummary, jauntSummary, houseHoleSummary);
        }
        public void ShowSummaryInMonthAndYear(IConsole console,int year , int month)
        {
            var list = _fileHandler.ReadAllExpenses().Where(e=> e.Date.Month == month&&e.Date.Year == year);
            double educationalSummary = 0;
            double jauntSummary = 0;
            double medicineSummary = 0;
            double houseHoleSummary = 0;
            foreach (var item in list)
            {
                if (item.Category == ExpenseCategory.Educational)
                    educationalSummary += item.Amount;
                else if (item.Category == ExpenseCategory.Jaunt)
                    jauntSummary += item.Amount;
                else if (item.Category == ExpenseCategory.Medicine)
                    medicineSummary += item.Amount;
                else if (item.Category == ExpenseCategory.HouseHold)
                    houseHoleSummary += item.Amount;
            }
            RenderChart(educationalSummary, medicineSummary, jauntSummary, houseHoleSummary);
        }
        public void ShowSummaryInMonth(IConsole console,int month)
        {
            var list = _fileHandler.ReadAllExpenses().Where(e=>e.Date.Month == month);
            double educationalSummary = 0;
            double jauntSummary = 0;
            double medicineSummary = 0;
            double houseHoleSummary = 0;
            foreach (var item in list)
            {
                if (item.Category == ExpenseCategory.Educational)
                    educationalSummary += item.Amount;
                else if (item.Category == ExpenseCategory.Jaunt)
                    jauntSummary += item.Amount;
                else if (item.Category == ExpenseCategory.Medicine)
                    medicineSummary += item.Amount;
                else if (item.Category == ExpenseCategory.HouseHold)
                    houseHoleSummary += item.Amount;
            }
            RenderChart(educationalSummary, medicineSummary, jauntSummary, houseHoleSummary);
        }
        public void ShowAllSummary(IConsole console) 
        {
            var list = _fileHandler.ReadAllExpenses();
            double educationalSummary = 0;
            double jauntSummary = 0;
            double medicineSummary = 0;
            double houseHoleSummary = 0;
            foreach (var item in list) 
            {
                if (item.Category == ExpenseCategory.Educational)
                    educationalSummary += item.Amount;
                else if (item.Category == ExpenseCategory.Jaunt)
                    jauntSummary += item.Amount;
                else if(item.Category == ExpenseCategory.Medicine)
                    medicineSummary += item.Amount;
                else if(item.Category == ExpenseCategory.HouseHold)
                    houseHoleSummary += item.Amount;
            }
            RenderChart(educationalSummary, medicineSummary, jauntSummary, houseHoleSummary);
        }
        private void RenderChart(double educationalSummary, double medicineSummary, double jauntSummary, double houseHoldSummary) 
        {
            AnsiConsole.Write(new BarChart()
                .Width(80)
                .Label("[green bold underline]Summary Of Expenses[/]")
                .CenterLabel()
                .AddItem("Educational",educationalSummary,Color.Green)
                .AddItem("Jaunt",jauntSummary,Color.Yellow3)
                .AddItem("HouseHole",houseHoldSummary,Color.Blue)
                .AddItem("Medicine", medicineSummary, Color.Red)) ;
        }
        private bool IsYearValid(int? year)
        {
            int minYear = 1950;
            int maxYear = DateTime.Now.Year ;//define current year

            return year >= minYear && year <= maxYear;
        }
        private bool IsValidMonth(int? month) 
        {
            if(month >12 && month <1)
                return false;
            return true;
        }
        private void Reset() 
        {
            this.Year = null;
            this.Month = null;
        }
    }
}
