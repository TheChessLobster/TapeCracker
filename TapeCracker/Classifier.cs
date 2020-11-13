using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapeCracker
{//KNN classifier
    public static class Classifier
    {
        public static int KNNClassCalc(double[] testoutputs, double[][] testInputs, double[] realInput)
        {

            return 1;
        }



        public static double EuclideanDistance(double x1, double y1, double x2, double y2)
         => Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
    }
}
