using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q5StronglyConnected: Processor
    {
        public Q5StronglyConnected(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);
        public bool[] visited;
        public long[] res;
        public long Solve(long nodeCount, long[][] edges)
        {
            res = new long[nodeCount];
            visited = new bool[nodeCount];
            for (int i = 0; i < nodeCount; i++)
                visited[i] = false;
            List<long>[] g = graph(nodeCount, edges);
            List<long>[] gr = ReverseGraph(nodeCount, edges);

            return SCC(nodeCount, g, gr); ;
        }
        public List<long>[] graph(long count, long[][] nodes)
        {
            List<long>[] graph = new List<long>[count];
            for (int i = 0; i < count; i++)
            {
                graph[i] = new List<long>();
            }
            foreach (var g in nodes)
            {
                graph[g[0] - 1].Add(g[1]);
            }
            return graph;
        }
        public List<long>[] ReverseGraph(long count, long[][] nodes)
        {
            List<long>[] graph = new List<long>[count];
            for (int i = 0; i < count; i++)
            {
                graph[i] = new List<long>();
            }
            foreach (var g in nodes)
            {
                graph[g[1] - 1].Add(g[0]);
            }
            return graph;
        }
        public long SCC(long nodeCount, List<long>[] g, List<long>[] gr)
        {
            long sccCount = 0;
            bool[] vist = new bool[nodeCount];
            ReversePost(vist, nodeCount, gr);
            foreach (var w in res)
            {
                if (!visited[w - 1])
                {
                    Explore(w, g);
                    sccCount++;
                }
            }
            return sccCount;
        }
        public void Explore(long v, List<long>[] g)
        {
            Stack<long> explore = new Stack<long>();
            explore.Push(v);
            long explored = 0;
            while (explore.Any())
            {
                explored = explore.Pop();
                visited[explored - 1] = true;
                foreach (var w in g[explored - 1])
                    if (!visited[w - 1])
                        explore.Push(w);
            }

        }
        public void ReversePost(bool[] vist, long nodeCount, List<long>[] g)
        {
            Stack<long> top = new Stack<long>();

            for (int i = 0; i < nodeCount; i++)
            {
                if (vist[i] == false)
                {
                    Post(i + 1, vist, top, g);
                }
            }
            int j = 0;
            while (top.Count != 0)
            {
                res[j++] = top.Pop();
            }

        }

        private void Post(long v, bool[] visited, Stack<long> top, List<long>[] g)
        {
            visited[v - 1] = true;
            for (int j = 0; j < g[v - 1].Count; j++)
            {
                if (!visited[g[v - 1][j] - 1])
                    Post(g[v - 1][j], visited, top, g);
            }
            top.Push(v);
        }
    }
}
