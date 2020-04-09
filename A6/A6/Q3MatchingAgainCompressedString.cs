using System;
using System.Collections.Generic;
using TestCommon;

namespace A6
{
    public class Q3MatchingAgainCompressedString : Processor
    {
        public Q3MatchingAgainCompressedString(string testDataName)
        : base(testDataName) { /*this.ExcludeTestCaseRangeInclusive(2, 30);*/ }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        /// <summary>
        /// Implement BetterBWMatching algorithm
        /// </summary>
        /// <param name="text"> A string BWT(Text) </param>
        /// <param name="n"> Number of patterns </param>
        /// <param name="patterns"> Collection of n strings Patterns </param>
        /// <returns> A list of integers, where the i-th integer corresponds
        /// to the number of substring matches of the i-th member of Patterns
        /// in Text. </returns>
        public long[] Solve(string text, long n, String[] patterns)
        {
            long[] res = new long[n];
            long[] ranks = new long[text.Length];
            // long[] ranks = new long[bwt.Length];
            Dictionary<char, long> counts = new Dictionary<char, long>();
            counts.Add('$', 0);
            counts.Add('A', 0);
            counts.Add('C', 0);
            counts.Add('G', 0);
            counts.Add('T', 0);

            for (int i = 0; i < text.Length; i++)
            {
                ranks[i] = counts[text[i]];
                counts[text[i]] += 1;

            }

            Dictionary<char, long> firstColumn = new Dictionary<char, long>();
            long ranker = 0;
            foreach (var item in counts)
            {
                firstColumn.Add(item.Key, ranker);
                ranker += item.Value;
            }

            for (int i = 0; i < patterns.Length; i++)
            {
                char c = patterns[i][patterns[i].Length - 1];
                long top = firstColumn[c];
                long bottom = firstColumn[c] + counts[c] - 1;

                res[i] = bwPatternMatching(text, patterns[i], top, bottom, firstColumn, ranks);
            }

            return res;
        }

        private long bwPatternMatching(string text, string pattern, long top, long bottom, Dictionary<char, long> firstColumn, long[] ranks)
        {
            long res = 0;

            for (int i = pattern.Length - 2; i >= 0; i--)
            {
                int t = int.MaxValue;
                int b = int.MinValue;

                for (long j = top; j <= bottom; j++)
                {
                    var symbol = text[(int)j];
                    if (symbol == pattern[(int)i])
                    {
                        if ((int)(firstColumn[symbol] + ranks[(int)j]) < t)
                            t = (int)(firstColumn[symbol] + ranks[(int)j]);

                        if ((int)(firstColumn[symbol] + ranks[(int)j]) > b)
                            b = (int)(firstColumn[symbol] + ranks[(int)j]);
                    }
                }
                top = t;
                bottom = b;
                if (t == int.MaxValue || b == int.MaxValue)
                {
                    return 0;
                }
            }
            res = bottom - top + 1;
            return res;
        }
    }
}
