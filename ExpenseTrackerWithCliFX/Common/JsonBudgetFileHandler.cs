using CliFx.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Common
{
    public class JsonBudgetFileHandler
    {
        private readonly string _budgetFile;
        public JsonBudgetFileHandler(string budgetFile) 
        {
            _budgetFile = budgetFile;
        }
        public void AddBudget(double amount, int month, int year)
        {
            var budgets = LoadBudget();
            var budget = new Budget(amount, month, year);
            budget.Id = budgets.Any() ? budgets.Max(budget => budget.Id) + 1 : 1;//autho generate ID
            budgets.Add(budget);
            
            SaveBudgets(budgets);
        }
        public void UpdateBudget( Budget budgerToUpdate) 
        {
            var budgets = LoadBudget();
            var existBudget = budgets.FirstOrDefault(e=>e.Id ==budgerToUpdate.Id);
            if (existBudget != null)
            {
                existBudget.Year = budgerToUpdate.Year;
                existBudget.Month = budgerToUpdate.Month;
                existBudget.Amount = budgerToUpdate.Amount;
                SaveBudgets(budgets);
            }
            else { throw new CommandException("Budget With ID : " + budgerToUpdate.Id + " not found"); }

        }
        public Budget GetBudgetById(int id) 
        {
            return LoadBudget().FirstOrDefault(e => e.Id == id);
        }
        public List<Budget> GetBudgetForYear(int year) 
        {
            var budgets = LoadBudget().Where(e => e.Year == year).ToList();
            return budgets;
        }
        public List<Budget> GetBudgetByMonth(int month)
        {
            var budgets = LoadBudget().Where(e => e.Month == month).ToList();
            return budgets;
        }
        public void DeleteBudget(int id) 
        {
            var budgets = LoadBudget();
            var budgetToRemove = budgets.FirstOrDefault(e => e.Id == id&&e.Year == DateTime.Now.Year);
            if (budgetToRemove != null)
            {
                budgets.Remove(budgetToRemove);
                SaveBudgets(budgets);
            }
            else 
            {
                throw new CommandException("Budget with ID "+id+" does not exist");
            }
        }
        public double GetAmountOfBudgetByMonthAndYear(int month,int year)
        {
            var budget = LoadBudget().FirstOrDefault(b => b.Month == month && b.Year == year);
            return budget.Amount;
        }
        public List<Budget> GetBudgets() 
        {
            var list = LoadBudget();
            SaveBudgets(list);
            return list;
        }
        public void SaveBudgets(List<Budget> budgets) 
        {
            var json = JsonConvert.SerializeObject(budgets,Formatting.Indented);
            File.WriteAllText( _budgetFile,json);
        }
        public List<Budget> LoadBudget() 
        {
            var jsonFile = File.ReadAllText(_budgetFile);
            return JsonConvert.DeserializeObject<List<Budget>>(jsonFile) ?? new List<Budget>();
        }
        public bool IsExist(int year, int month) 
        {
            bool exsit = LoadBudget().Where(e => e.Year == year && e.Month == month).Any();
            return exsit;
        }
        public bool IsVliadToUpdate(int year, int month, int id) 
        {
            bool isValid = LoadBudget().Where(e => e.Year == year && e.Month == month).SkipWhile(e=>e.Id==id).Any();
            return isValid;
        }
    }
}
