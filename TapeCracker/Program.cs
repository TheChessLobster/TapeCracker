using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
            //Using TapeType pull in the List of columns needed for ETL

            //Verify or grab from file containing needed loantype columns (store in a csv file within the project?)
            var LoanTape = Extractor.GetLoans(args[0]);
            //Verify column header row from args (later calculate dynamically)
            var HeaderCol = Extractor.FindAndLoadHeader(LoanTape);

            

            var test = 0;
            //Send HeaderCol,
        }

        public static string[] ColumnGrab(string DealType)
        {
            switch (DealType)
            {
                case "MCIRT":
                    return CSVSingleLineReader(0);
                case "CIRT":
                    return CSVSingleLineReader(1);
            }
            return new string[] { "you", "messed", "up" };//Default no match error
        }

        public static string[] CSVSingleLineReader(int line)
        {
            string[] allLines = File.ReadAllLines("C:\\Users\\thech\\Desktop\\School\\Capstone\\DealType.csv");
            string[] dealLine = allLines[line].Split('|'); // error if less lines, check allLines.Length
            return dealLine;
        }
    }
}
