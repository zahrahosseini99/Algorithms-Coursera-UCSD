using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { /*this.ExcludeTestCaseRangeInclusive(49, 50);*/ }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);

        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {
            DirectedGraph g = new DirectedGraph(edges, NodeCount);

            long s, t;
            long i = 0;
            long[] res = new long[QueriesCount];

            foreach (var q in Queries)
            {
                long result = 0;
                s = q[0] - 1;
                t = q[1] - 1;
                if (s == t)
                    result = 0;
                else
                    result = g.BidirectionalDijkstra(s, t);
                if (result == int.MaxValue)
                    res[i++] = -1;
                else
                    res[i++] = result;
            }

            return res;
        }
    }
}
