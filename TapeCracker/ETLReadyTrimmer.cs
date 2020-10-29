using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Use this class to perform final data validation on 'ETL ready' csv file
//The goal is to use some form of machine learning to automatically correct errors in the data (if possible)
//The point of this project is to limit human interaction with the LoanTape as much as possible
namespace TapeCracker
{
    public class ETLReadyTrimmer
    {
        public static int[] ColumnLocator(string[] KeyColumns, string[] LoanTapeValueColumns)
        {
            double currMax = -1;
            int[] ColumnLocs = new int[KeyColumns.Length];
            Dictionary<string, string> Matches = new Dictionary<string, string>(); 
            for (int i = 1; i < KeyColumns.Length; i++)
            {
                for (int j = 0; j < LoanTapeValueColumns.Length; j++)
                {
                    if (LoanTapeValueColumns[j] != "")
                    {
                        var newMax = MatchCalc(KeyColumns[i], LoanTapeValueColumns[j]);
                        if (newMax > currMax)
                        {
                            currMax = newMax;
                            ColumnLocs[i] = j;
                        }
                    }
                }
                //Post max match is found
                Matches.Add(KeyColumns[i], LoanTapeValueColumns[ColumnLocs[i]]);              
                LoanTapeValueColumns[ColumnLocs[i]] = "";// to prevent duplicate matches
                currMax = -1; //reset max to math. min to make sure it doesn't skip stuff.
            }
            double totalDistance = 0;
            foreach(KeyValuePair<string,string> kvp in Matches)
            {
                var currDistance = MatchCalc(kvp.Key, kvp.Value);
                totalDistance += currDistance;

            }
            var averageDistance = totalDistance / Matches.Count(); //If below 40 the match sucks
            return ColumnLocs;
        }
        public static double MatchCalc(string source, string target)
        {
            if (source == target) return 1;
            double steps = Convert.ToDouble(LevDistance(source, target));
            return (1 - (steps / (double)Math.Max(source.Length, target.Length)));
        }
        public static int LevDistance(string string1, string string2)
        {
            if (string1 == string2) return string1.Length;//Exact match
            int sourceChars = string1.Length;
            int targetChars = string2.Length;
            int[,] distance = new int[sourceChars + 1, targetChars + 1];
            for (int i = 0; i <= sourceChars; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetChars; distance[0, j] = j++) ;
            for (int i = 1; i <= sourceChars; i++)
            {
                for (int j = 1; j <= targetChars; j++)
                {
                    int cost = (string2[j - 1] == string1[i - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }
            return distance[sourceChars, targetChars];
        }
    }
}
