using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Common
{
    public class Budget
    {
        public int Id { get;set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Amount { get; set; }
        public Budget() { }
        public Budget(double amount, int month, int year)
        {
            Year = year;
            Month = month;
            Amount = amount;
        }
        public Budget(int id, int year, int month, double amount)
        {
            Id = id;
            Year = year;
            Month = month;
            Amount = amount;
        }
    }
}
