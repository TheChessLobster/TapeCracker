using System;
using System.Linq;
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
            var classString = "";
            var GoalColumns = ColumnGrab("CIRT", "C:\\Users\\thech\\Desktop\\DealType.csv");
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
            var TrimmedHeaderRow = TrimHeaderRow(HeaderRow);           
            var realVector = ETLReadyTrimmer.KNNVectors(GoalColumns, TrimmedHeaderRow);

            //INCLUDED TEST CASES FOR KNN
            //DUMMY VECTOR OF 100 PERCENT "CIRT" Match, WITH 90 PERCENT HIGH MATCH
            double[] CIRTDouble = { 1, .90 };
            var CIRTtest = Classifier.KNNClassCalc(numClasses, KNNTestData, CIRTDouble, 5); //Should classify as '1'

            //DUMMY VECTOR OF 20 PERCENT "CIRT" MATCH, WITH 20 PERCENT HIGH MATCH
            double[] MCIPDouble = { .2, .1 };
            var MCIPtest = Classifier.KNNClassCalc(numClasses, KNNTestData, MCIPDouble, 5); //Should classify as '2'



            var TestClassification = Classifier.KNNClassCalc(numClasses, KNNTestData, realVector, 5); //Real input(MCIRT) should classify as 0
            //0 = MCIRT, 1 = CIRT, 2 = MCIP
            //Based on classification of LoanTape (MCIP,MCIRT,CIRT), change the 'GoalColumns' value, then continue to validation.
            switch (TestClassification) {
                case 0:
                    classString = "MCIRT";
                    break;
                case 1:
                    classString = "CIRT";
                    break;
                case 2:
                    classString = "MCIP";
                    break;
                        }
            var GoalRow = ColumnGrab(classString, "C:\\Users\\thech\\Desktop\\DealType.csv");
            var LocList = ETLReadyTrimmer.ColumnLocator(GoalRow, TrimHeaderRow(HeaderRow));       
            var Loans = Extractor.GetLoans(args[0]).ToList();
            Loans.RemoveRange(0, 4);
            for(int j = 0; j < Loans.Count(); j++)
            {
                Loans[j] = Loans[j][0].Split(Convert.ToChar(","));        
            }
      
       }

        public static string[] ColumnGrab(string DealType, string path)
        {
            switch (DealType)
            {
                case "CIRT":
                    return CSVSingleLineReader(5, path, Convert.ToChar(","));
                case "MCIRT":
                    return CSVSingleLineReader(4, path, Convert.ToChar(","));
                case "CIP":
                    return CSVSingleLineReader(3, path, Convert.ToChar(","));
            }
            return new string[] { };
        }

        public static string[] CSVSingleLineReader(int line, string path, char delimiter)
        {
            string[] allLines = File.ReadAllLines(path);
            string[] dealLine = allLines[line].Split(delimiter); 
            return dealLine;
        }

        public static string[] TrimHeaderRow(string[] HeaderRow)
        {
            string[] TrimmedHeaderRow = new string[62];
            for (int i = 0; i < 62; i++)
            {
                TrimmedHeaderRow[i] = HeaderRow[i];
            }
            return TrimmedHeaderRow;
        }
}
}
