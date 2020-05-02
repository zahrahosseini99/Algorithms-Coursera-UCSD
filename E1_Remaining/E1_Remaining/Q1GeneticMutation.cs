using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q1GeneticMutation : Processor
    {
        public Q1GeneticMutation(string testDataName) : base(testDataName) { }
        public override string Process(string inStr)
            => TestTools.Process(inStr, (Func<string, string, string>)Solve);


        static int no_of_chars = 256;

        public string Solve(string firstDNA, string secondDNA)
        {
            
            long[] countf = new long[no_of_chars];
            long[] counts = new long[no_of_chars];
            for (int i = 0; i < firstDNA.Length; i++)
            {
                countf[firstDNA[i]]++;
            }
            for (int i = 0; i < secondDNA.Length; i++)
            {
                counts[secondDNA[i]]++;
            }
            if (secondDNA.Length != firstDNA.Length)
                return (-1).ToString();
            for (int i = 0; i < secondDNA.Length; i++)
            {
                if (countf[firstDNA[i]] != counts[firstDNA[i]])
                    return (-1).ToString();
            }
            return 1.ToString();
        }
    }
}
