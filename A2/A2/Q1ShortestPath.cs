using System;
using TestCommon;

namespace A2
{
    public class Q1ShortestPath : Processor
    {
        public Q1ShortestPath(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long[][], long, long, long>)Solve);
        long[] dist;
        long[] prev ;
        public long Solve(long NodeCount, long[][] edges, 
                          long StartNode,  long EndNode)
        {
            dist = new long[NodeCount];
            prev = new long[NodeCount];
            UnDirectedGraph flights = new UnDirectedGraph(edges, NodeCount);
            flights.BFS(StartNode,dist, prev);
            return flights.ReconstructPath(StartNode, EndNode, prev);
        }
    }
}
