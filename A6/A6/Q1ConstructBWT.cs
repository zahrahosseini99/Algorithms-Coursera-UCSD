using System;
using TestCommon;

namespace A6
{
    public class Q1ConstructBWT : Processor
    {
        public Q1ConstructBWT(string testDataName)
        : base(testDataName) {}

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Construct the Burrows–Wheeler transform of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> BWT(Text) </returns>
        public string Solve(string text)
        {
        
            int[] sufArray = buildSuffixArray(text, text.Length);
            string res = "";
            for (int i = 0; i < text.Length; i++)
            {
                res += text[(sufArray[i] - 1 + text.Length) % text.Length];
            }
            return res;
        }

        private int[] buildSuffixArray(string text, int length)
        {
            suffix[] res = new suffix[length];
            int[] result = new int[length];
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

    internal class suffix
    {
        public int index;
        public string suf;
        public suffix(int i, string s)
        {
            index = i;
            suf = s;
        }
    }
}
