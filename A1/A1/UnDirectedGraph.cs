using System.Collections.Generic;
using System.Linq;

namespace A1
{
    public class UnDirectedGraph
    {
        public long[][] Nodes;
        public long NodeCount;
        public List<long>[] graph;
        public UnDirectedGraph(long[][] nodes, long nodecount)
        {
            Nodes = nodes;
            NodeCount = nodecount;
            graph = new List<long>[nodecount];
            for (int i = 0; i < nodecount; i++)
            {
                graph[i] = new List<long>();
            }
            foreach (var g in nodes)
            {
                graph[g[0] - 1].Add(g[1]);
                graph[g[1] - 1].Add(g[0]);
            }
        }
        public long BFS(long start, long end)
        {
            long res = 0;
            Queue<long> bfs = new Queue<long>();
            List<long> g = new List<long>();
            bool[] visited = new bool[NodeCount];

            bfs.Enqueue(start);

            while (bfs.Count != 0)
            {
                long index = bfs.Peek();
                if (index == end)
                    return 1;
                g.Add(bfs.Dequeue());
                visited[index - 1] = true;

                foreach (var a in graph[index - 1])
                {
                    if (!visited[a - 1])
                        bfs.Enqueue(a);
                }
            }
            return res;
        }
        public void Explore(long v,bool[] visited,long[] CCnum,long cc)
        {
            Stack<long> explore = new Stack<long>();
            explore.Push(v);
            long explored = 0;
            while (explore.Any())
            {
                explored = explore.Pop();
                visited[explored - 1] = true;
                CCnum[explored - 1] = cc;
                foreach (var w in graph[explored - 1])
                    if (!visited[w - 1])
                        explore.Push(w);
            }

        }
        public long DFS(bool[] visited, long[] CCnum, long cc)
        {
            cc = 0;
            for (int i = 0; i < graph.Length; i++)
            {
                if (!visited[i])
                {
                    Explore(i + 1,visited,CCnum,cc);
                    cc++;
                }

            }
            return cc;
        }
    }
}