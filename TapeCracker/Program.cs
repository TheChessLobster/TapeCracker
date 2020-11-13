using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NumSharp;
using NumSharp.Utilities;
using NumSharp.Generic;
using Accord.MachineLearning;
using Accord;
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
            var TapeType = args[1];//Hopefully don't need this anymore
            var GoalColumns = ColumnGrab(TapeType, args[2]);
            //Grab and load in the test data, run through the ETLReadyTrimmer to create our test dataset
            var TestSchemas = Extractor.GetTestSchemasOrClasses("C:\\Users\\thech\\Desktop\\TestSchemas.csv").ToList();
            var SchemaClasses = Extractor.GetTestSchemasOrClasses("C:\\Users\\thech\\Desktop\\DealTypeClass.csv").ToList();
            double[] numClasses = new double[SchemaClasses.Count()];
            double[][] KNNTestData = new double[TestSchemas.Count()][];
            for (int i = 0;i<TestSchemas.Count();i++)
            {
                KNNTestData[i] = ETLReadyTrimmer.KNNVectors(GoalColumns,TestSchemas[i]);
                numClasses[i] = Convert.ToDouble(SchemaClasses[i][0]);
            }
            var HeaderRow = CSVSingleLineReader(3, args[0], Convert.ToChar(","));
            string[] TrimmedHeaderRow = new string[62];
            for(int i = 0; i < 62; i++)
            {
                TrimmedHeaderRow[i] = HeaderRow[i];
            }
            var realVector = ETLReadyTrimmer.KNNVectors(GoalColumns, TrimmedHeaderRow);//0 = CIRT, 1 = MCIRT, 2 = MCIP

            var TestClassification = Classifier.KNNClassCalc(numClasses, KNNTestData, realVector, 5);//How to decide K?


            //Based on classification of LoanTape (MCIP,MCIRT,CIRT), change the 'GoalColumns' value, then continue to validation.
            var LocList = ETLReadyTrimmer.ColumnLocator(GoalColumns, TrimmedHeaderRow);
           // var check = 0;

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
                case "CIP":
                    return CSVSingleLineReader(3, path, Convert.ToChar(","));
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
