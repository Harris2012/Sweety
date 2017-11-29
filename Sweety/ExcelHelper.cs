using Sweety.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety
{
    class ExcelHelper
    {
        public List<ExcelTable> ReadFromExcel(string excelFilePath)
        {
            throw new NotImplementedException();
        }

        public static void WriteToExcel(string excelFilePath, Dictionary<string, List<ExcelTable>> entityList)
        {
            foreach (var entity in entityList)
            {
                
            }
        }
    }
}
