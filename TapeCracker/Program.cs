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
            string[] trimmedHeaderRow = new string[62];
            for (int i = 0; i < trimmedHeaderRow.Length; i++){
                    trimmedHeaderRow[i] = HeaderRow[i];
            }
            var teststrings = ETLReadyTrimmer.MatchCalc("LoanNumber", "LoanIDNumber");//These 2 have an 80 percent cut-off value
            var LocList = ETLReadyTrimmer.ColumnLocator(GoalColumns, trimmedHeaderRow);
            //Use those array locs to pull from 'LoanTape' item, just below the header
            //Perform validation in the 'validator' using statistical rules i get from andrew
            var stop = 0;
            //Use ML to keep record of error of probability
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
    }
}
