using System;
using TestCommon;

namespace A6
{
    public class Q4ConstructSuffixArray : Processor
    {
        public Q4ConstructSuffixArray(string testDataName) 
        : base(testDataName) {  }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        /// <summary>
        /// Construct the suffix array of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> SuffixArray(Text), that is, the list of starting positions
        /// (0-based) of sorted suffixes separated by spaces </returns>
        public long[] Solve(string text)
        {
            long[] sufArray = buildSuffixArray(text, text.Length);
            return sufArray;
        }
        private long[] buildSuffixArray(string text, int length)
        {
            suffix[] res = new suffix[length];
            long[] result = new long[length];
            for (int i = 0; i < length; i++)
            {
                res[i] = new suffix(i, text.Substring(i));

            }

            Array.Sort(res, (x, y) => String.Compare(x.suf, y.suf));
            for (int i = 0; i < length; i++)
            {
                result[i] = res[i].index;
            }

            return result;
        }
    }
}
