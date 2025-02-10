using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Common
{
    //Expese entity class for make works simple
    public class ExpenseEntity
    {
        
        //Propertie
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateOnly Date { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ExpenseCategory Category { get; set; }
        public ExpenseEntity() { }
        public ExpenseEntity(int id, string description,DateOnly date, double amount, ExpenseCategory category)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
            Category = category;
        }
        public ExpenseEntity(string description,DateOnly date, double amount, ExpenseCategory category) 
        {
            Description = description;
            Date = date;
            Amount = amount;
            Category = category;
        }
    }
    //Enum category list
    public enum ExpenseCategory 
    {
        Educational,
        Medicine,
        Jaunt,
        HouseHold
    }
}
