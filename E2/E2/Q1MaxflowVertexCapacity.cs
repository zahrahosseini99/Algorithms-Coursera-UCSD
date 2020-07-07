using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace E2
{
    public class Q1MaxflowVertexCapacity : Processor
    {
        public Q1MaxflowVertexCapacity(string testDataName) : base(testDataName)
        { /*this.ExcludeTestCaseRangeInclusive(16, 41);*/}

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][],long[] , long, long, long>)Solve);

        public virtual long Solve(long nodeCount, 
            long edgeCount, long[][] edges, long[] nodeWeight, 
            long startNode , long endNode)
        {
            long maxflow = ComputeMaxFlow(nodeCount, edgeCount, edges, nodeWeight);
            return maxflow;
        }
        private long ComputeMaxFlow(long nodeCount, long edgeCount, long[][] edges, long[] nodeWeight)
        {
            //first check for path?
            long maxFlow = 0;
            bool IsTherePath = false;
            long[,] adj = ConstructGraph(edges,nodeCount, edgeCount,edgeCount,nodeWeight);
            long newNodeCount = edgeCount * 2 + 1;
            bool[] visited = new bool[newNodeCount];
            long[] preV = new long[newNodeCount];
            preV = Enumerable.Repeat((long)-1, (int)newNodeCount).ToArray();
            IsTherePath = BFS(adj, visited, preV, 0, newNodeCount- 1);
            while (IsTherePath)
            {
                long u = newNodeCount - 1;
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
                visited = Enumerable.Repeat(false, (int)newNodeCount).ToArray();
                preV = Enumerable.Repeat((long)-1, (int)newNodeCount).ToArray();
                IsTherePath = BFS(adj, visited, preV, 0, newNodeCount - 1);
            }
            return maxFlow;
        }

        private long[,] ConstructGraph(long[][] edges, long nodeCount, long edgeCount1, long edgeCount2, long[] nodeWeight)
        {
            long counter = nodeCount;
            long[,] adj = new long[edgeCount1 + edgeCount1 + 1, edgeCount1 + edgeCount1 + 1];
            foreach (var e in edges)
            {
                
                adj[e[0] - 1, counter] = nodeWeight[e[0] - 1];
                if (e[1] == nodeCount)
                {
                    adj[counter, edgeCount1 + edgeCount1] = e[2];
                }
                else
                adj[counter, e[1] - 1] = e[2];
                counter++;
            }
            adj[nodeCount - 1, counter] = nodeWeight[nodeCount - 1];
            return adj;
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
