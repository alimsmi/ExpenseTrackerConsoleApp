using CliFx;
using ExpenseTrackerWithCliFX.Commands;
using ExpenseTrackerWithCliFX.Commands.BudgetCommands;
using ExpenseTrackerWithCliFX.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var con = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
                .Build();

            string jsonLocation = con["jsonFileLocation"]+ "\\expensesStorage.json";
            var textPath = new TextPath("Expense Path : "+jsonLocation)
                .RootColor(Color.Red)
                .SeparatorColor(Color.Green)
                .StemColor(Color.Blue)
                .LeafColor(Color.Yellow);
            AnsiConsole.Write(textPath);
            AnsiConsole.WriteLine();
            string budgetFileLocation = con["jsonFileLocation"] + "\\budget.json";
            var budgetPath = new TextPath("Budget Path : "+budgetFileLocation)
                .RootColor(Color.Red)
                .SeparatorColor(Color.Green)
                .StemColor(Color.Blue)
                .LeafColor(Color.Yellow);
            AnsiConsole.Write(budgetPath);
            AnsiConsole.WriteLine();
            IServiceCollection services = new ServiceCollection();
            //Services
            services.AddSingleton<JsonFileHandler>(_ => new JsonFileHandler(jsonLocation));
            services.AddScoped<JsonBudgetFileHandler>(_ => new JsonBudgetFileHandler(budgetFileLocation));
            services.AddScoped<ListCommand>();
            services.AddScoped<AddCommand>();
            services.AddScoped<SummaryCommand>();
            services.AddScoped<UpdateCommand>();
            services.AddScoped<ExportCommand>();
            services.AddScoped<BudgetCommand>();
            services.AddScoped<AddBudgetCommand>();
            services.AddScoped<UpdateBudgetCommand>();
            services.AddScoped<DeleteBudgetCommand>();


            var serviceProvider = services.BuildServiceProvider();
            AnsiConsole.Write(new FigletText("ExpenseTracker"));
            while (true)
            {
                string input = Console.ReadLine();
                var arg = input.Split();
                await new CliApplicationBuilder()
                    .AddCommandsFromThisAssembly()
                    .UseTypeActivator(serviceProvider.GetService)
                    .Build()
                    .RunAsync(arg);
            }
        }
    }
}
