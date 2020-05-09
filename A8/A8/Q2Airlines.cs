using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q2Airlines : Processor
    {
        public Q2Airlines(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long, long[][], long[]>)Solve);

        public virtual long[] Solve(long flightCount, long crewCount, long[][] info)
        {
           
            
            long nodeCount = flightCount + crewCount + 2;
            long[,] adj = new long[nodeCount, nodeCount];

            ConstructGraph(info,adj, nodeCount,flightCount,crewCount);

            ComputeMaxFlow(adj, nodeCount);

            long[] result = new long[flightCount];
            result = Enumerable.Repeat((long)-1, (int)flightCount).ToArray();

            for (long i = flightCount + 1; i <= flightCount + crewCount; i++)
                for (int j = 1; j <= flightCount; j++)
                    if (adj[i, j] == 1)
                        result[j - 1] = i - flightCount;

            return result;
        }

        private void ConstructGraph(long[][] info,long[,] adj, long nodeCount,long flightCount, long crewCount)
        {
            for (int i = 0; i < info.Length; i++)
            {
                for (int j = 0; j < info[0].Length; j++)
                {
                    if (info[i][j] == 1)
                        adj[i + 1, j + flightCount + 1] = 1;
                }
            }

            for (int i = 1; i <= flightCount; i++)
                adj[0, i] = 1;

            for (long i = flightCount + 1; i <= flightCount + crewCount; i++)
                adj[i, flightCount + crewCount + 1] = 1;
        }

        private void ComputeMaxFlow(long[,] adj, long nodeCount)
        {
            long[] preV = new long[nodeCount];
            preV = Enumerable.Repeat((long)-1, (int)nodeCount).ToArray();

            bool IstherePath = false;
            long maxFlow = 0;
            bool[] visited = new bool[nodeCount];
            IstherePath = BFS(adj, visited, preV, 0, nodeCount - 1);

            while (IstherePath)
            {
                long u = nodeCount - 1;
                long min = int.MaxValue;
                List<long> path = new List<long>();
                while (u != 0)
                {
                    if (adj[preV[u], u] < min)
                        min = adj[preV[u], u];
                    path.Add(u);
                    u = preV[u];
                }
                path.Add(0);
                for (int i = path.Count - 1; i > 0; i--)
                {
                    adj[path[i], path[i - 1]] -= min;
                    adj[path[i - 1], path[i]] += min;
                }
                maxFlow += min;
                for (int i = 0; i < nodeCount; i++)
                {
                    visited[i] = false;
                    preV[i] = (long)-1;
                }

                IstherePath = BFS(adj, visited, preV, 0, nodeCount - 1);
            }
        }

        private bool BFS(long[,] adj, bool[] visited, long[] preV, int s, long t)
        {
            Queue<long> q = new Queue<long>();
            q.Enqueue(s);
            visited[s] = true;
            while (q.Count != 0)
            {
                long u = q.Dequeue();
                if (u == t)
                    return true;
                for (int v = 0; v <= t; v++)
                {
                    if (adj[u, v] > 0 && !visited[v])
                    {
                        preV[v] = u;
                        visited[v] = true;
                        q.Enqueue(v);
                    }
                }

            }
            return false;
        }
    }
}
