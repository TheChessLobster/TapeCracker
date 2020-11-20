using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapeCracker
{//KNN classifier
    public static class Classifier
    {
        public static int KNNClassCalc(int[] testoutputs, double[][] testInputs, double[] realInput, int neighborcount)
        {
            var DistList = distances(realInput,testInputs);
            var KClosest = FindKClosestVotes(DistList, neighborcount,testoutputs);
            var Classification = KClosest.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;//LINQ way to grab 'mode' of a List
            return Classification;
        }

        public static double[] distances(double[] realinput, double[][] testinputs)
        {
            double[] distances = new double[testinputs.Count()];
            double realx1 = realinput[0];
            double realy1 = realinput[1];
            for (int i = 0; i < testinputs.Count();i++)
            {
                distances[i] = EuclideanDistance(realx1, realy1, testinputs[i][0], testinputs[i][1]);
            }
            return distances;
        }

        public static int[] FindKClosestVotes(double[] DistList, int neighborCount,int[] testoutputs)
        {
            var voteCount = 0;
            int[] VoteList = new int[neighborCount];
            for(int i = 0; i < DistList.Count(); i++)
            {
                var MinValue = DistList.Min();
                if (DistList[i] == MinValue) 
                {
                    VoteList[voteCount] = Convert.ToInt32(testoutputs[i]);
                    voteCount++;
                    DistList[i] = 9999999;
                    if(voteCount == neighborCount) { break; }
                }
            }
            return VoteList;
        }

        public static double EuclideanDistance(double x1, double y1, double x2, double y2)
         => Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
    }
}
