using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace A3
{
    public class DirectedGraph
    {
        public long[][] Edges;
        public long NodeCount;
        public List<long>[] Graph;
        public List<long>[] GraphR;
        public long[,] Weight;
        public long[,] WeightR;
        public long sign;
        public DirectedGraph(long[][] edges, long nodecount)
        {
            Edges = edges;
            NodeCount = nodecount;
            Graph = new List<long>[nodecount];
            GraphR = new List<long>[nodecount];
            Weight = new long[nodecount, nodecount];
            WeightR = new long[nodecount, nodecount];
            for (int i = 0; i < nodecount; i++)
            {
                Graph[i] = new List<long>();
                GraphR[i] = new List<long>();
            }
            foreach (var g in edges)
            {
                Graph[g[0] - 1].Add(g[1]);
                GraphR[g[1] - 1].Add(g[0]);
                Weight[g[0] - 1, g[1] - 1] = g[2];
                if (g[2] < 0)
                    sign = 1;
                WeightR[g[1] - 1, g[0] - 1] = g[2];
            }

        }

        public long BidirectionalDijkstra(long s, long t)
        {
            long[] dist = new long[NodeCount];
            long[] distR = new long[NodeCount];
            bool[] proc = new bool[NodeCount];
            bool[] procR = new bool[NodeCount];
            List<long> relaxed = new List<long>();
            List<long> relaxedR = new List<long>();

            for (int i = 0; i < NodeCount; i++)
            {
                dist[i] = int.MaxValue;
                distR[i] = int.MaxValue;
                proc[i] = false;
                procR[i] = false;
            }

            dist[s] = 0;
            distR[t] = 0;

            do
            {

                long v = DequeueMin(dist, proc);
                Process(v, dist, proc, relaxed, Graph, Weight);
                if (procR[v - 1])
                    return ShortestPath(s, dist, relaxed, t, distR);

                long vR = DequeueMin(distR, procR);
                Process(vR, distR, procR, relaxedR, GraphR, WeightR);
                if (proc[vR - 1])
                    return ShortestPath(t, distR, relaxedR, s, dist);

            } while (true);


        }
        private long DequeueMin(long[] dist, bool[] proc)
        {
            long temp = long.MaxValue;
            long index = -1;

            for (int i = 0; i < dist.Length; i++)
            {
                if (dist[i] < temp && !proc[i])
                {
                    temp = dist[i];
                    index = i;
                }
            }
            return index + 1;
        }
        public long ShortestPath(long s, long[] dist, List<long> relaxed,
            long t, long[] distR)
        {
            long distance = int.MaxValue;
            long uBest = -1;

            foreach (var v in relaxed)
            {
                if (dist[v - 1] + distR[v - 1] < distance)
                {
                    uBest = v;
                    distance = dist[v - 1] + distR[v - 1];
                }
            }
            return distance;

        }
        private void Process(long u, long[] dist, bool[] proc,
            List<long> relaxed, List<long>[] Graph, long[,] Weight)
        {
            foreach (var v in Graph[u - 1])
            {
                if (dist[v - 1] > dist[u - 1] + Weight[u - 1, v - 1])
                {
                    dist[v - 1] = dist[u - 1] + Weight[u - 1, v - 1];

                    relaxed.Add(v);
                }

            }
            proc[u - 1] = true;
            relaxed.Add(u);
        }


        public void Dijkstra(long start, long[] dist, long[] prev)
        {
            for (int i = 0; i < NodeCount; i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = -1;
            }
            dist[start - 1] = 0;
            SimplePriorityQueue<long, long> pq = new SimplePriorityQueue<long, long>();
            for (int i = 0; i < NodeCount; i++)
            {
                pq.Enqueue(i + 1, dist[i]);
            }
            while (pq.Count() > 0)
            {
                var u = pq.Dequeue();
                foreach (var v in Graph[u - 1])
                {
                    if (dist[v - 1] > dist[u - 1] + Weight[u - 1, v - 1])
                    {
                        dist[v - 1] = dist[u - 1] + Weight[u - 1, v - 1];
                        prev[v - 1] = u;
                        pq.Enqueue(v, dist[v - 1]);
                    }

                }
            }
        }

        public bool HasNegativeCycle(long start, long[] dist)
        {

            dist[start] = 0;
            bool relax = false;

            for (long i = 1; i < NodeCount; i++)
            {
                relax = false;
                foreach (var e in Edges)
                {
                    if (dist[e[0] - 1] != int.MaxValue && dist[e[1] - 1] > dist[e[0] - 1] + Weight[e[0] - 1, e[1] - 1])
                    {
                        dist[e[1] - 1] = dist[e[0] - 1] + Weight[e[0] - 1, e[1] - 1];
                        relax = true;
                    }
                }
            }

            return relax;
        }
        public long IsReach(long[] dist)
        {
            if (sign == 0)
                return 0;
            bool res = false;
            for (int i = 0; i < NodeCount; i++)
            {
                if (dist[i] == int.MaxValue)
                {
                    res = HasNegativeCycle(i, dist);
                }

            }
            if (res)
                return 1;
            else
                return 0;
        }



    }
}