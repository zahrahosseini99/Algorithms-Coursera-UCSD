using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;
using System.Linq;

namespace Exam1
{
    public class Q4Vaccine : Processor
    {
        public Q4Vaccine(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<string, string, string>)Solve);

        public string Solve(string dna, string pattern)
        {
            string text = pattern + "$" + dna;
            int n = text.Length;
            int pLength = pattern.Length;
            int dnaLength = dna.Length;
            int[] zArray = new int[n];

            BuildZArray(text, zArray);

            string reverseDna = new string(dna.ToCharArray().Reverse().ToArray());
            string reversePattern = new string(pattern.ToCharArray().Reverse().ToArray());
            string reverseText = reversePattern + "$" + reverseDna;
            int[] reverseZArray = new int[n];

            BuildZArray(reverseText, reverseZArray);

          
            List<int> res = new List<int>();
           
            int textTraveller = pLength + 1;
            int patterntraveller = 0;
            while (textTraveller<= dnaLength + 1)
            {
                if (zArray[textTraveller] + reverseZArray[dnaLength - patterntraveller+1] >= pLength - 1)
                {
                    res.Add(textTraveller - pLength - 1);

                }
                patterntraveller++;
                textTraveller++;
            }
           
            if (res.Count == 0)
                return "No Match!";
            return string.Join(" ", res);


        }

        private static void BuildZArray(string text, int[] Zarray)
        {
            var n = text.Length;
            var length = 0;
            var z = 0;
            for (var i = 1; i < n; ++i)
            {
                if (i <= z)
                {
                    Zarray[i] = Math.Min(z - i + 1, Zarray[i - length]);
                }
                   
                while (i + Zarray[i] < n && text[Zarray[i]] == text[i + Zarray[i]])
                {
                    ++Zarray[i];
                }
                   
                if (i + Zarray[i] - 1 <= z)
                    continue;

                length = i;
                z = i + Zarray[i] - 1;
            }
        }
    }
}
