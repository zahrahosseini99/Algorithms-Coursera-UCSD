using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q1FindAllOccur : Processor
    {
        public Q1FindAllOccur(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, string pattern)
        {
            return FindAllOccurrences(pattern, text);
        }

        private long[] FindAllOccurrences(string pattern, string text)
        {
            string concat = pattern + "$" + text;
            long[] prefixfunction = new long[concat.Length];
            prefixfunction = ComputePrefixFunction(concat);
            List<long> result = new List<long>();
            for (int i = pattern.Length + 1; i < concat.Length; i++)
            {
                if (prefixfunction[i] == pattern.Length)
                    result.Add(i-2*(pattern.Length));
           }
            if (result.Count == 0)
                result.Add(-1);
            return result.ToArray();
        }

        private long[] ComputePrefixFunction(string p)
        {
            long[] prefixFunc = new long[p.Length];
            prefixFunc[0] = 0;
            long border = 0;
            for (int i = 1; i < p.Length; i++)
            {
                while ((border>0) && (p[i]!=p[(int)border]) )
                {
                    border = prefixFunc[border - 1];
                }
                if (p[i] == p[(int)border])
                {
                    border = border + 1;
                }
                else
                {
                    border = 0;
                }
                prefixFunc[i] = border;
            }
            return prefixFunc;
        }
    }
}
