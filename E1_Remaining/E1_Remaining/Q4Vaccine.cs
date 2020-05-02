using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q4Vaccine : Processor
    {
        public Q4Vaccine(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<string, string, string>)Solve);

        public string Solve(string dna, string pattern)
        {
         
            List<int> res = new List<int>();
            res = search(dna, pattern);
            if (res.Count == 0)
                return "No Match!";
            return string.Join(" ", res);
        }
        public static List<int> search(string text,
                          string pattern)
        {
            List<int> res = new List<int>();
           
            string concat = pattern + "$" + text;

            int l = concat.Length;

            int[] Z = new int[l];

            getZarr(concat, Z);

            for (int i = 0; i < l; ++i)
            {

                if (Z[i] == pattern.Length || Z[i] == pattern.Length-1 )
                {
                   res.Add(i - pattern.Length - 1);
                }
            }
            return res;
        }

        private static void getZarr(string str,
                                    int[] Z)
        {

            int n = str.Length;

            int L = 0, R = 0;

            for (int i = 1; i < n; ++i)
            {

                if (i > R)
                {
                    L = R = i;

                    while (R < n && str[R - L] == str[R])
                    {
                        R++;
                    }

                    Z[i] = R - L;
                    R--;

                }
                else
                {

                    int k = i - L;

                    if (Z[k] < R - i + 1)
                    {
                        Z[i] = Z[k];
                    }

                    else
                    {

                        L = i;
                        while (R < n && str[R - L] == str[R])
                        {
                            R++;
                        }

                        Z[i] = R - L;
                        R--;
                    }
                }
            }
        }
       
    }
}
