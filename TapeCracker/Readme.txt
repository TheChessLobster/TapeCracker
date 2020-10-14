Evan Seghers
Milliman/UWW-Whitewater Project

CURRENT TO DO
10/14/2020

1.Get Header Row from LoanTape MCIRT 20193 Loaded Into the platform
2.Confirm header mapping is taken in from the DealType.csv file
3.In ETLReadyTrimmer class, for each Header mapping, find the best LevDistance match, and the column location, add to a dictionary to be returned at the end of this method. 
4.In the program then, from the loan tape, grab just the numerical data from the LevDistance matched columns. 
5.Then move to the 'validator' class which will return output on 1. Whether the match was correct, and 2. If the data beneath the columns is accurate.

Going forward, step 5 will absorb this data, and attempt to understand what SHOULD the mappings be numerically going forward, and rely less and less on naming conventions and more on numbers.
