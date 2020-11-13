using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NumSharp;
using NumSharp.Utilities;
using NumSharp.Generic;
//Evan Seghers
//UW-Whitewater Student
//Milliman Employee
//Loan Tape Cracker Project

namespace TapeCracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Inputs neededv
            // LoanTapePath||TapeType||DealType csv(list of needed inputs)
            // C:\Users\thech\Desktop\MCIRT20193 MCIRT C:\Users\thech\Desktop\DealType
            var TapeType = args[1];
            var GoalColumns = ColumnGrab(TapeType, args[2]);
            var HeaderRow = CSVSingleLineReader(3, args[0], Convert.ToChar(","));
            List<string[]> loans = new List<string[]>();
            string[] trimmedHeaderRow = new string[HeaderRow.Length];
            for (int i = 0; i < trimmedHeaderRow.Length; i++){
                    trimmedHeaderRow[i] = HeaderRow[i];
            }
            var LocList = ETLReadyTrimmer.ColumnLocator(GoalColumns, trimmedHeaderRow);
            var Loans = Extractor.GetLoans(args[0]).ToList();
            Loans.RemoveRange(0, 4);//Header rows
            for(int j = 0; j < Loans.Count(); j++)
            {
                Loans[j] = Loans[j][0].Split(Convert.ToChar(","));
           
            }
            var check = 0;
            //loop through and delete all
      
        }

        public static string[] ColumnGrab(string DealType, string path)
        {
            switch (DealType)
            {
                case "MCIRT":
                    return CSVSingleLineReader(5, path, Convert.ToChar(","));
                case "CIRT":
                    return CSVSingleLineReader(4, path, Convert.ToChar(","));
            }
            return new string[] { };
        }

        public static string[] CSVSingleLineReader(int line, string path, char delimiter)
        {
            string[] allLines = File.ReadAllLines(path);
            string[] dealLine = allLines[line].Split(delimiter); // error if less lines, check allLines.Length
            return dealLine;
        }

        public static int CSVLineCounter(int startLine, string path)
        {
            return File.ReadAllLines(path).Count() - startLine;
        }
        public static string[] LoanValuesSplitter(string[] Loans)
        {
            Loans[0].Split(Convert.ToChar(","));
            return Loans;
        }
    }
}
