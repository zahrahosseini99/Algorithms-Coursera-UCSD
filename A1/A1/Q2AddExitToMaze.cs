using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q2AddExitToMaze : Processor
    {
        public Q2AddExitToMaze(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);
        public bool[] visited;
        public long[] CCnum;
        public long cc;
        public long Solve(long nodeCount, long[][] edges)
        {
            visited = new bool[nodeCount];
            CCnum = new long[nodeCount];

            for (int i = 0; i < nodeCount; i++)
                visited[i] = false;
            UnDirectedGraph g = new UnDirectedGraph(edges, nodeCount);
            return g.DFS(visited, CCnum, cc);
        }
    }
}
