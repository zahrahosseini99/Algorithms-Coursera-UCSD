using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q3Stocks : Processor
    {
        public Q3Stocks(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long stockCount, long pointCount, long[][] matrix)
        {
            //create a matrix graph of matrix
            long[,] info = new long[stockCount, stockCount];
            for (int i = 0; i < info.GetLength(0); i++)
                for (int j = 0; j < info.GetLength(1); j++)
                    info[i, j] = 1;
            ConstructGraph((int)pointCount, matrix, info);
            return CountNumberOfSheets(info, stockCount);
        }
        private void ConstructGraph(int pointCount, long[][] matrix, long[,] info)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    for (int k = 0; k < pointCount; k++)
                    {
                        if (matrix[i][k] >= matrix[j][k])
                            info[i, j] = 0;
                    }
                }

            }

        }
        private long CountNumberOfSheets(long[,] info, long stockCount)
        {
            long n = 2 * stockCount + 2;
            long[,] adj = new long[n, n];
            long maxFlow = 0;
            FillAdjMatrix(info, adj, stockCount);
            maxFlow = ComputeMaxFlow(adj, n);
            return stockCount - maxFlow;
        }

        private void FillAdjMatrix(long[,] info, long[,] adj, long stockCount)
        {
            for (int i = 0; i < info.GetLength(0); i++)
            {
                for (int j = 0; j < info.GetLength(1); j++)
                {
                    if (info[i, j] == 1)
                        adj[i + 1, j + stockCount + 1] = 1;
                }
            }


            for (int i = 1; i <= stockCount; i++)
                adj[0, i] = 1;

            for (long i = stockCount + 1; i <= stockCount + stockCount; i++)
                adj[i, stockCount + stockCount + 1] = 1;
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
