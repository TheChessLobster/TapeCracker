using System;
using System.Linq;
using System.IO;
using Accord.Statistics;
using Accord.Statistics.Analysis;
using System.Collections.Generic;
using Accord.Math;

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
            var catTest = ETLReadyTrimmer.MatchCalc("cat", "dog"); //0
            var specialTest = ETLReadyTrimmer.MatchCalc("special", "especially");  //.7776
            var GoalColumns = ColumnGrab("CIRT", "C:\\Users\\thech\\OneDrive\\Desktop\\DealType.csv"); //Grabs a 'CIRT' loantype
            var TestSchemas = Extractor.GetTestSchemasOrClasses("C:\\Users\\thech\\OneDrive\\Desktop\\TestSchemas.csv").ToList();
            var TestingSchemas = Extractor.GetTestSchemasOrClasses("C:\\Users\\thech\\OneDrive\\Desktop\\TestingSchemas.csv").ToList();
            var TestingSchemasClean = Extractor.GetTestSchemasOrClasses("C:\\Users\\thech\\OneDrive\\Desktop\\TestingSchemasClean.csv").ToList();
            var SchemaClasses = Extractor.GetTestSchemasOrClasses("C:\\Users\\thech\\OneDrive\\Desktop\\DealTypeClass.csv").ToList();//Real classification
            int[] numClasses = new int[SchemaClasses.Count()];
            double[][] KNNTestData = new double[TestSchemas.Count()][];
            double[][] KNNTestingData = new double[TestingSchemas.Count()][];
            double[][] KNNTestingDataClean = new double[TestingSchemasClean.Count()][];
            for (int i = 0;i < TestSchemas.Count();i++)
            {
                KNNTestData[i] = ETLReadyTrimmer.KNNVectors(GoalColumns,TestSchemas[i]);
                numClasses[i] = Convert.ToInt32(SchemaClasses[i][0]);
                KNNTestingData[i] = ETLReadyTrimmer.KNNVectors(GoalColumns, TestingSchemas[i]);
                KNNTestingDataClean[i] = ETLReadyTrimmer.KNNVectors(GoalColumns, TestingSchemasClean[i]);
            }
            int[] RealClassification = new int[KNNTestingData.Count()];
            int[] RealCleanClassification = new int[KNNTestingDataClean.Count()];
            for(int i = 0; i < KNNTestingData.Count(); i++)
            {
                RealClassification[i] = Classifier.KNNClassCalc(numClasses, KNNTestData, KNNTestingData[i], 5);
                RealCleanClassification[i] = Classifier.KNNClassCalc(numClasses, KNNTestData, KNNTestingDataClean[i], 5);
            }

            //Use ML Data or manual method to compute the Confusion Matrix, Precision, and Recall

            var confusionMatrix = new GeneralConfusionMatrix(classes: 3, expected: numClasses, predicted: RealClassification);
            Console.WriteLine("Precision: " + confusionMatrix.Precision[0] + " " + confusionMatrix.Precision[1] + " " + confusionMatrix.Precision[2]);
            Console.WriteLine("Recall: " + confusionMatrix.Recall[0] + " " + confusionMatrix.Recall[1] + " " + confusionMatrix.Recall[2]);
            Console.WriteLine("Total Accuracy: " + confusionMatrix.Accuracy);
            int[,] check = confusionMatrix.Matrix;
            PrintMatrix(check);

            var cleanConfusionMatrix = new GeneralConfusionMatrix(classes: 3, expected: numClasses, predicted: RealCleanClassification);
            Console.WriteLine(" Precision: " + cleanConfusionMatrix.Precision[0] + " " + cleanConfusionMatrix.Precision[1] + " " + cleanConfusionMatrix.Precision[2]);
            Console.WriteLine( "Recall: " + cleanConfusionMatrix.Recall[0] + " " + cleanConfusionMatrix.Recall[1] + " " + cleanConfusionMatrix.Recall[2]);
            Console.WriteLine( "Total Accuracy: " + cleanConfusionMatrix.Accuracy);
            int[,] check2 = cleanConfusionMatrix.Matrix;
            PrintMatrix(check2);

            var HeaderRow = CSVSingleLineReader(3, args[0], Convert.ToChar(","));
            var TrimmedHeaderRow = TrimHeaderRow(HeaderRow);           
            var realVector = ETLReadyTrimmer.KNNVectors(GoalColumns, TrimmedHeaderRow);

            var TestClassification = Classifier.KNNClassCalc(numClasses, KNNTestData, realVector, 5); //Real input(MCIRT) should classify as 0
            //0 = MCIRT, 1 = CIRT, 2 = MCIP
            //Based on classification of LoanTape (MCIP,MCIRT,CIRT), change the 'GoalColumns' value, then continue to validation.
            //Based on the 'real vector' classification, grab the correct needed columns
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
            var GoalRow = ColumnGrab(classString, "C:\\Users\\thech\\OneDrive\\Desktop\\DealType.csv");
            var LocList = ETLReadyTrimmer.ColumnLocator(GoalRow, TrimHeaderRow(HeaderRow));       
            var Loans = Extractor.GetLoans(args[0]).ToList();
            Loans.RemoveRange(0, 4);
            var doublecheck = 0;
            for(int j = 0; j < Loans.Count(); j++)
            {
                Loans[j] = Loans[j][0].Split(Convert.ToChar(","));
                Loans[j] = Loans[j].Take(70).ToArray();
            }
            var triplecheck = 0;
            List<string[]> SelectedLoans = new List<string[]>();
            //Now we just only grab the columns that are in section [1] of the 'LocList' array, and bam we have our values needed (unvalidated)
            for(int j = 0;j < Loans.Count; j++)
            {
                int count = 0;
                for (int k = 0;k < LocList.Count(); k++)
                {
                    var ValtoAdd = Loans[j][LocList[k]];
                    Loans[j][k] = ValtoAdd;
                }
                Loans[j] = Loans[j].Take(49).ToArray();
            }
            var finalcheck = 0;
            //Grab 3 'test vectors'
            double[][] DataKNNTestVectors = new double[3][];
            var MCIRTsarray = CSVSingleLineReader(3, "C:\\Users\\thech\\OneDrive\\Desktop\\TypeDataValidation.csv", Convert.ToChar(","));
            double[] MCIRTdarray = Array.ConvertAll(MCIRTsarray, Double.Parse);
            DataKNNTestVectors[0] = MCIRTdarray;

            var CIRTsarray = CSVSingleLineReader(5, "C:\\Users\\thech\\OneDrive\\Desktop\\TypeDataValidation.csv", Convert.ToChar(","));
            double[] CIRTdarray = Array.ConvertAll(CIRTsarray, Double.Parse);
            DataKNNTestVectors[1] = CIRTdarray;

            var MCIPsarray = CSVSingleLineReader(1, "C:\\Users\\thech\\OneDrive\\Desktop\\TypeDataValidation.csv", Convert.ToChar(","));
            double[] MCIPdarray = Array.ConvertAll(MCIPsarray, Double.Parse);
            DataKNNTestVectors[2] = MCIPdarray;
            //in dataKNNTestVectors 0 = MCIRT, 1 = CIRT, 2 = MCIP
            double amountCount = 0;
            double LTVCount = 0;
            double[] realDataVector = new double[2];
            switch (TestClassification)
            {
                case 0://MCIRT   (2,34)
                    for(int i = 0;i< Loans.Count(); i++)
                    {
                        amountCount += Convert.ToDouble(Loans[i][2]);
                        LTVCount += Convert.ToDouble(Loans[i][31]);
                    }
                    amountCount /= Loans.Count();
                    LTVCount /= Loans.Count();
                    realDataVector = new double[] { amountCount, LTVCount };
                    break;
                case 1://CIRT     (2,31)
                    for (int i = 0; i < Loans.Count(); i++)
                    {
                        amountCount += Convert.ToDouble(Loans[i][2]);
                        LTVCount += Convert.ToDouble(Loans[i][31]);
                    }
                    amountCount /= Loans.Count();
                    LTVCount /= Loans.Count();
                    realDataVector = new double[] { amountCount, LTVCount };
                    break;
                case 2://MCIP     (2,29)
                    for (int i = 0; i < Loans.Count(); i++)
                    {
                        amountCount += Convert.ToDouble(Loans[i][2]);
                        LTVCount += Convert.ToDouble(Loans[i][29]);
                    }
                    amountCount /= Loans.Count();
                    LTVCount /= Loans.Count();
                    realDataVector = new double[] { amountCount, LTVCount };
                    break;
            }
            //Grab values on our 1 'real vector, based on our classified dealtype
            //create a switch statement based on dealtype that grabs our 3 chosen things
            var DataClassification = Classifier.KNNClassCalc(new int[] { 0, 1, 2 }, DataKNNTestVectors, realDataVector, 1);

            //if string classification != data classification, remove data level mistake and re-classify. 
            if (DataClassification != TestClassification)
            {
                Console.Write("Data does not match the schema classification expectations, please check csv and re-run");
            }
            //now validate the data based on our finally correct data type, and remove any outliers, possibly replace with average value.
            else if( DataClassification == TestClassification)
            {
                Loans = Validator.ValidateLoans(Loans);
                //Now that the data is completely classified correctly, and cleaned, output final csv, and add our classification values/schema to our TestSchemas
                WriteToClassificationFile(Loans);
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
                case "MCIP":
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

        public static void WriteToClassificationFile(List<string[]> Loans)
        {           
            string folder = @"C:\Users\thech\";
            string fileName = "output.txt";
            string fullPath = folder + fileName;
            string[] mushedLoans = new string[Loans.Count()];
            string temp = "";
            for(int j = 0; j < Loans.Count(); j++)
            {
                for(int k = 0; k<Loans[j].Length; k++)
                {
                    temp += Loans[j][k] + ",";
                }
                mushedLoans[j] = temp;
                temp = "";
            }
            string[] toWrite = mushedLoans;
            File.WriteAllLines(fullPath, toWrite);
            string readText = File.ReadAllText(fullPath);
            Console.WriteLine(readText);
        }
        public static void PrintMatrix(int[,] matrix)
        {

            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            for (int i = 0; i < columns; i++)
            {
                if (i == 0)
                {
                    Console.Write("    0 1 2 3\n");
                    Console.WriteLine("  +---------+");
                }
                for (int j = 0; j < rows; j++)
                {
                    if (j == 0) Console.Write(i.ToString() + " | ");
                    Console.Write(matrix[i, j] + " ");
                    if (j == rows - 1) Console.Write("|");
                }
                Console.WriteLine();
                if (i == columns - 1) Console.Write("  +---------+");
            }
            Console.Write("\n");
        }
        
}
}
