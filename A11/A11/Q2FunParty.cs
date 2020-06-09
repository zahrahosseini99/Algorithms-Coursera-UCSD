using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A11
{
    public class Q2FunParty : Processor
    {
        public Q2FunParty(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long[], long[][], long>)Solve);
        public long[] d;
        public virtual long Solve(long n, long[] funFactors, long[][] hierarchy)
        {
            List<long>[] tree = new List<long>[n];
            constructTree(tree, hierarchy, n);

            d = new long[n];

            for (long i = 0; i < n; i++)
            {
                d[i] = long.MaxValue;

            }
            return funParty(0, -1, funFactors, tree);
        }

        private long funParty(long v, long parent, long[] funFactors, List<long>[] tree)
        {
            if (d[v] == long.MaxValue)
            {
                if (tree[v].Count == 1)
                    d[v] = funFactors[v];
                else
                {
                    long m1 = funFactors[v];
                    foreach (var u in tree[v])
                    {
                        if (u == parent)
                            continue;
                        foreach (var w in tree[u])
                        {
                            if (w != v)
                                m1 += funParty(w, u, funFactors, tree);
                        }
                    }
                    long m0 = 0;
                    foreach (var u in tree[v])
                    {
                        if (u != parent)
                            m0 += funParty(u, v, funFactors, tree);
                    }
                    d[v] = Math.Max(m1, m0);
                }

            }

            return d[v];
        }

        private void constructTree(List<long>[] tree, long[][] hierarchy, long n)
        {

            for (int i = 0; i < tree.Length; i++)
            {
                tree[i] = new List<long>();
            }
            foreach (var h in hierarchy)
            {
                tree[h[0] - 1].Add(h[1] - 1);
                tree[h[1] - 1].Add(h[0] - 1);
            }

        }
    }
}
