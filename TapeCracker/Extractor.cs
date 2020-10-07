using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
namespace TapeCracker
{
    public class Extractor
    {
        public static Tuple<List<string>, List<string>> GetLoansAndPrepayProvisions(string loansFile, string prepayFile)
            => Tuple.Create(File.ReadAllLines(loansFile).ToList(), File.ReadAllLines(prepayFile).ToList());

        public static IEnumerable<string[]> GetLoans(string loansFile)
            => File.ReadAllLines(loansFile).Select(f => f.Split('|'));//Due to Loan Tapes tending to be pipe delimited. 

    }
}
