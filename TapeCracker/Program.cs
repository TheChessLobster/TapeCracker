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

        
            //Get the header of our loanTape and run through the ETLReadyTrimmer to find our 'test' object, and predict it's classification

            //Once we know its classification 



            //var GoalColumns = ColumnGrab(TapeType, args[2]);//Grab 'GoalColumns' based on the decided classification of the LoanTape
                            //Currently this is set up so it assumes it's an MCIRT loan type but this is mostly a testing decision, but is our 'test against' tape type
            var HeaderRow = CSVSingleLineReader(3, args[0], Convert.ToChar(","));
            string[] TrimmedHeaderRow = new string[70];
            for(int i = 0; i < 70; i++)
            {
                TrimmedHeaderRow[i] = HeaderRow[i];
            }
            //var TestVector = ETLReadyTrimmer.KNNVectors(GoalColumns, TrimmedHeaderRow);//use these vectors and KNN loaded data to classify our data

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
