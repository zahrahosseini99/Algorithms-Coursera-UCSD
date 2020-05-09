using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            //first check for path?
            long maxFlow = 0;
            bool IsTherePath = false;
            long[,] adj = new long[nodeCount, nodeCount];
            foreach (var e in edges)
                adj[e[0] - 1, e[1] - 1] += e[2];
            bool[] visited = new bool[nodeCount];
            long[] preV = new long[nodeCount];
            preV = Enumerable.Repeat((long)-1, (int)nodeCount).ToArray();
            IsTherePath = BFS(adj, visited, preV, 0, nodeCount - 1);
            while (IsTherePath)
            {
                long u = nodeCount - 1;
                long min = int.MaxValue;
                List<long> path = new List<long>();
                while (u!=0)
                {
                    if (adj[preV[u], u] < min)
                        min = adj[preV[u], u];
                    path.Add(u);
                    u = preV[u];
                }
                path.Add(0);
                for (int i = path.Count-1; i > 0; i--)
                {
                    adj[path[i], path[i - 1]] -= min;
                    adj[path[i - 1], path[i]] += min;
                }
                maxFlow += min;
                visited = Enumerable.Repeat(false,(int) nodeCount).ToArray();
                preV = Enumerable.Repeat((long)-1, (int)nodeCount).ToArray();
                IsTherePath = BFS(adj, visited, preV, 0, nodeCount - 1);
            }
            return maxFlow;
        }

        private bool BFS(long[,] adj, bool[] visited, long[] preV , int s, long t)
        {
            Queue<long> q = new Queue<long>();
            q.Enqueue(s);
            visited[s] = true;
            while (q.Count != 0)
            {
                long u = q.Dequeue();
                if (u == t)
                    return true;
                for (int v = 0; v <=t; v++)
                {
                    if(adj[u,v]>0 && !visited[v])
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
