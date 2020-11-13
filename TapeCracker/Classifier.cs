using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapeCracker
{//KNN classifier
    public static class Classifier
    {
        public static int KNNClassCalc(double[] testoutputs, double[][] testInputs, double[] realInput, double neighborcount)
        {
            //Calculate the euclidean distances from realInput to all testInput vals
            var DistList = distances(realInput,testInputs);
            //Find the 5 closest values, and their classes from the 'testoutputs' matching index
            
            //find the mode classification of those 5

            //declare our realInput's real classification to be that.
            return 1;
        }

        public static double[] distances(double[] realinput, double[][] testinputs)
        {//should be in place, index wise, no need to hold the index.
            double[] distances = new double[testinputs.Count()];
            double realx1 = realinput[0];
            double realy1 = realinput[1];
            for (int i = 0; i < testinputs.Count();i++)
            {
                distances[i] = EuclideanDistance(realx1, realy1, testinputs[i][0], testinputs[i][1]);
            }
            return distances;
        }
        public static double EuclideanDistance(double x1, double y1, double x2, double y2)
         => Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
    }
}
