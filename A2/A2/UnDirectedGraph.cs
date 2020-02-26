using System;
using System.Collections.Generic;
using System.Linq;

namespace A2
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
        public void BFS(long start, long[] dist, long[] prev)
        {

            for (int i = 0; i < NodeCount; i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = -1;
            }
            dist[start - 1] = 0;

            Queue<long> q = new Queue<long>();

            q.Enqueue(start);

            while (q.Count != 0)
            {
                long index = q.Dequeue();

                foreach (var a in graph[index - 1])
                {

                    if (dist[a - 1] == int.MaxValue)
                    {
                        q.Enqueue(a);
                        dist[a - 1] = dist[index - 1] + 1;
                        prev[a - 1] = index;
                    }


                }
            }

        }


        public long ReconstructPath(long start, long end, long[] prev)

        {

            List<long> res = new List<long>();
            while (end != start)
            {
                res.Add(end);
                if (end == -1)
                    return end;
                end = prev[end - 1];
            }
            return res.Count;
        }
        public long isBipartite()
        {
            long[] color = new long[NodeCount];
            for (int i = 0; i < NodeCount; i++)
                color[i] = -1;
            for (int i = 0; i < NodeCount; i++)
            {
                if (color[i] == -1)
                    if (BipartiteBFS(i, color) == false)
                        return 0;
            }
            return 1;
        }

        private bool BipartiteBFS(long s, long[] color)
        {
            color[s] = 1;
            Queue<long> q = new Queue<long>();
            q.Enqueue(s + 1);
            while (q.Count != 0)
            {
                long u = q.Peek();
                q.Dequeue();
                for (int i = 0; i < graph[u - 1].Count; i++)
                {
                    if (color[graph[u - 1][i] - 1] == -1)
                    {
                        color[graph[u - 1][i] - 1] = 1 - color[u - 1];
                        q.Enqueue(graph[u - 1][i]);
                    }
                    else if (color[graph[u - 1][i] - 1] == color[u - 1])
                        return false;
                }
            }
            return true;
        }

    }
}