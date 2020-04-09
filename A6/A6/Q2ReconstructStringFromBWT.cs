using System;
using System.Collections.Generic;
using TestCommon;
using System.Linq;

namespace A6
{
    public class Q2ReconstructStringFromBWT : Processor
    {
        public Q2ReconstructStringFromBWT(string testDataName)
        : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Reconstruct a string from its Burrows–Wheeler transform
        /// </summary>
        /// <param name="bwt"> A string Transform with a single “$” sign </param>
        /// <returns> The string Text such that BWT(Text) = Transform.
        /// (There exists a unique such string.) </returns>
        public string Solve(string bwt)
        {
            long[] ranks = new long[bwt.Length];
            Dictionary<char, long> counts = new Dictionary<char, long>();
            counts.Add('$', 0);
            counts.Add('A', 0);
            counts.Add('C', 0);
            counts.Add('G', 0);
            counts.Add('T', 0);

            for (int i = 0; i < bwt.Length; i++)
            {
                ranks[i] = counts[bwt[i]];
                counts[bwt[i]]+=1;

            }

            Dictionary<char, long> firstColumn = new Dictionary<char, long>();
            long ranker = 0;
            foreach (var item in counts)
            {
                firstColumn.Add(item.Key, ranker);
                ranker += item.Value;
            }

            return BwInverse(bwt, ranks, firstColumn);
        }

        private string BwInverse(string bwt, long[] ranks, Dictionary<char, long> firstColumn)
        {
            Stack<char> res = new Stack<char>();

            long i = 0;
            res.Push('$');
            while (bwt[(int)i] != '$')
            {
                res.Push(bwt[(int)i]);
                i = firstColumn[bwt[(int)i]] + ranks[i];
            }
            return new string(res.ToArray());
        }
    }
}
