using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace TapeCracker
{
    public class Extractor
    {
        public static IEnumerable<string[]> GetLoans(string loansFile)
            => File.ReadAllLines(loansFile).Select(f => f.Split('|'));//Due to Loan Tapes tending to be pipe delimited. 

        public static IEnumerable<string[]> GetTestSchemasOrClasses(string loansFile)
            => File.ReadAllLines(loansFile).Select(f => f.Split(','));//Due to Loan Tapes tending to be pipe delimited. 

        public static string[] FindAndLoadHeader(IEnumerable<string[]> LoanTape)
        {
            var LoanTapeList = LoanTape.ToList();
            for (int i = 0; i < LoanTape.Count(); i++)
            {
                var FieldCount = LoanTapeList[i].Count();
                if (FieldCount > 5) return LoanTapeList[i];
            }
            return LoanTapeList[0];
        }
    }
}
