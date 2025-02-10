using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerWithCliFX.Common
{
    public static class CSVFileHandler
    {
        private static string _csvFileInDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ Path.DirectorySeparatorChar+"output.csv";
        public static void Export(List<ExpenseEntity> entities) 
        {
            if(!File.Exists(_csvFileInDesktop))
                File.Create(_csvFileInDesktop).Close();
            using (var writer = new StreamWriter(_csvFileInDesktop))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<ExpenseEntity>();
                csv.NextRecord();

                csv.WriteRecords(entities);
                
            }
        }
    }
}
