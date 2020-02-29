using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q1MinCost : Processor
    {
        public Q1MinCost(string testDataName) : base(testDataName) {}

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);

        long[] dist;
        long[] prev;
        public long Solve(long nodeCount, long[][] edges, long startNode, long endNode)
        {
            dist = new long[nodeCount];
            prev = new long[nodeCount];

            DirectedGraph flight = new DirectedGraph(edges, nodeCount);
            flight.Dijkstra(startNode, dist, prev);

            if (dist[endNode - 1] < int.MaxValue)
                return dist[endNode - 1];
            else
                return -1;
        }
    }
}
