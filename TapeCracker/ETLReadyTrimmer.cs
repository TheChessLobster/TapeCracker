﻿using System;
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
        public static int LevDistance(string string1, string string2)
        {
            if ((string1 == null) || (string2 == null)) return 0;
            if ((string1.Length == 0) || (string2.Length == 0)) return 0;
            if (string1 == string2) return string1.Length;

            int sourceChars = string1.Length;
            int targetChars = string2.Length;

            // Step 1
            if (sourceChars == 0)
                return targetChars;

            if (targetChars == 0)
                return sourceChars;

            int[,] distance = new int[sourceChars + 1, targetChars + 1];

            // Step 2
            for (int i = 0; i <= sourceChars; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetChars; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceChars; i++)
            {
                for (int j = 1; j <= targetChars; j++)
                {
                    // Step 3
                    int cost = (string2[j - 1] == string1[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceChars, targetChars];
        }
        public static double MatchCalc(string source, string target)
        {
            if (source == target) return 1.0;

            int stepsToSame = LevDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
    }
}
