using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace E2
{
    public class Q1MaxflowVertexCapacity : Processor
    {
        public Q1MaxflowVertexCapacity(string testDataName) : base(testDataName)
        { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], long, long, long>)Solve);

        public virtual long Solve(long nodeCount,
            long edgeCount, long[][] edges, long[] nodeWeight,
            long startNode, long endNode)
        {
            long n = nodeCount * 2;
            long[,] adj = new long[n, n];
            ConstructGraph(adj, edgeCount, nodeCount, edges, nodeWeight);
            return ComputeMaxFlow(adj, n);
        }

        private void ConstructGraph(long[,] adj, long edgeCount,
            long nodeCount, long[][] edges, long[] nodeWeight)
        {
            long counter = nodeCount;
            foreach (var e in edges)
            {
                adj[e[0] - 1, counter + e[0] - 1] = nodeWeight[e[0] - 1];
                adj[counter + e[0] - 1, e[1] - 1] = e[2];
            }
            adj[nodeCount - 1, counter + nodeCount - 1] = nodeWeight[nodeCount - 1];
        }

        private long ComputeMaxFlow(long[,] adj, long nodeCount)
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
            return maxFlow;
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
