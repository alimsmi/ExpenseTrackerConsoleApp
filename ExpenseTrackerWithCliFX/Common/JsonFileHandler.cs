using CliFx.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Common
{
    public class JsonFileHandler
    {
        private readonly string _filePath;
        public JsonFileHandler(string filePath)
        {
            _filePath = filePath;
        }
        public List<ExpenseEntity> ReadAllExpenses() 
        {
            var jsonFile = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<ExpenseEntity>>(jsonFile) ?? new List<ExpenseEntity>();
        }
        //Read specific list by category
        public List<ExpenseEntity> ReadAllExpensesByCategory(ExpenseCategory category)
        {
            return ReadAllExpenses().Where(C => C.Category == category).ToList();
        }
        //Helper method to write back all entities to file
        private void WriteExpenses(List<ExpenseEntity> entities) 
        {
            string json = JsonConvert.SerializeObject(entities,Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
        //Method to add expense
        public void AddExpense(ExpenseEntity entity) 
        {
            var expenses = ReadAllExpenses();
            entity.Id = expenses.Any() ? expenses.Max(e => e.Id) + 1 : 1;//Auto generate ID
            expenses.Add(entity);
            WriteExpenses(expenses);
        }
        //Method to get expense by id 
        public ExpenseEntity GetExpenseById(int id) 
        {
            var expenses = ReadAllExpenses();
            return expenses.FirstOrDefault(e => e.Id == id);
        }
        //Method to update expense 
        public void UpdateExpenseByEntity(ExpenseEntity updatedEntity) 
        {
            var expenses = ReadAllExpenses();
            var exsitedEntity = expenses.FirstOrDefault(e=>e.Id ==  updatedEntity.Id);
            if (exsitedEntity == null)
                throw new CommandException("Can not found expense with ID : "+updatedEntity.Id);
            
            exsitedEntity.Description = updatedEntity.Description;
            exsitedEntity.Amount = updatedEntity.Amount;
            exsitedEntity.Category = updatedEntity.Category;
            WriteExpenses(expenses);
        }
        //Method to remove an entity
        public void DeleteExpenseById(int id) 
        {
            var expenses = ReadAllExpenses();
            var expenseToRemove = expenses.FirstOrDefault(e => e.Id == id);
            if (expenseToRemove != null)
            {
                expenses.Remove(expenseToRemove);
                WriteExpenses(expenses);
            }
            else 
            {
                throw new ArgumentNullException(nameof(expenseToRemove));
            }

        }
    }
}
