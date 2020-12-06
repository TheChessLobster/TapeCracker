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
    public static class Validator
    {
        public static List<string[]> ValidateLoans(List<string[]> Loans)
        {
            //within the loop figure out using error catching, if this is a double, or a string

            for(int i = 0; i< Loans.Count; i++)
            {
                for(int j = 0;j < Loans[i].Length; j++)
                {
                    try
                    {
                        var myDouble = Convert.ToDouble(Loans[i][j]);
                        Loans[i][j] = ValidateDoubles(myDouble,i,j).ToString();

                    }
                    catch(FormatException f)
                    {
                        Loans[i][j] = ValidateString(Loans[i][j],i,j);                      
                        continue;
                    }
                }
                if(i/1000 == 1.0000 & i< 10000)
                {
                    Console.WriteLine(i + " loans completed");
                }
                //Console.WriteLine(i);
            }
            var check = Loans;
            return Loans;
        }

        public static double ValidateDoubles(double loanDouble,double loanindex,double columnindex)
        {
            //Validate that based on previous values in this index, that this value is good for this index, with a dictionary
            if(loanDouble < 0|| loanDouble > 100000000000000|| loanDouble == null)
            {
                loanDouble = 0;
                Console.WriteLine("There is an errant value for loan " + loanindex + " at column " + columnindex);
            }
            return loanDouble;
        }

        public static string ValidateString(string loanString, double loanindex, double columnindex)
        {
            loanString = loanString.ToUpper();
            if(loanString == "") {
                loanString = "u";
            }
            return loanString;
        }
    }
}
