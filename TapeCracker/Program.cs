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
        public static void Main(string[] args)
        {
            //Inputs needed
            // LoanTapePath||TapeType||DealType csv(list of needed inputs)
            // C:\Users\thech\Desktop\MCIRT20193 MCIRT C:\Users\thech\Desktop\DealType
            var TapeType = args[1];
            var GoalColumns = ColumnGrab(TapeType, args[2]);
            var HeaderRow = CSVSingleLineReader(3, args[0], Convert.ToChar(","));
            var LoanCount = CSVLineCounter(3, args[0]);
            List<string[]> loans = new List<string[]>();
            string[] trimmedHeaderRow = new string[62];
            for (int i = 0; i < trimmedHeaderRow.Length; i++){
                    trimmedHeaderRow[i] = HeaderRow[i];
            }
            var LocList = ETLReadyTrimmer.ColumnLocator(GoalColumns, trimmedHeaderRow);
            //We now know which columns of the csv contain the data that we need, so grab this data, and print it off to a new csv
            for(int i = 4; i < LoanCount; i++)
            {
                var nonParsed = CSVSingleLineReader(i, args[0], Convert.ToChar(","));
                var Parsed = new string[LocList.Count()];
                for(int j = 0;j < nonParsed.Count(); j++)
                {
                    for(int k = 0; k < LocList.Count(); k++)
                    {
                        if(j == LocList[k]) { Parsed[k] = nonParsed[j]; }
                    }
                }
                loans.Add(Parsed);
            }
            var added = loans;
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
    }
}
