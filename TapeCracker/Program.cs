using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Evan Seghers
//UW-Whitewater Student
//Milliman Employee
//Loan Tape Cracker Project

namespace TapeCracker
{
    public class Program
    {
        //Possible args list
        //FilePath to loanTape being inputted (later edit this to take in multiple dealtapes at once)
        //DealType list (make an enum for this)
        public static void Main(string[] args)
        {
            //Verify Loan tape type from Args
            var TapeType = args[1];
            //Verify or grab from file containing needed loantype columns (store in a csv file within the project?)
            var LoanTape = Extractor.GetLoans(args[0]);
            //Verify column header row from args (later calculate dynamically)
            var HeaderCol = Extractor.FindAndLoadHeader(LoanTape);
            //Send HeaderCol,
        }
    }
}
