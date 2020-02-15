using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace A1
{
    public class Q3Acyclic : Processor
    {
        public Q3Acyclic(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            DirectedGraph g = new DirectedGraph(edges, nodeCount);
            List<long> First = new List<long>();
            List<long> Middle = new List<long>();
            List<long> Second = new List<long>();
            long res = 0;

            for (int i = 0; i < nodeCount; i++)
            {
                First.Add(i + 1);
            }
            long v = 0;
            while (First.Count > 0)
            {
                v = First.First();
                First.RemoveAt(0);
                if (g.HasCycle(v, First, Middle, Second))
                    res = 1;
            }
            return res;
        }
    }
}