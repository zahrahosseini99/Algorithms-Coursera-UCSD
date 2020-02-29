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
            GraphR= new List<long>[nodecount];
            Weight = new long[nodecount, nodecount];
            WeightR= new long[nodecount, nodecount]; 
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
          for (int i = 0; i < NodeCount; i++)
            {
                dist[i] = int.MaxValue;
                distR[i] = int.MaxValue;
           }
            dist[s] = 0;
            distR[t] = 0;
            List<long> proc = new List<long>();
            List<long> procR = new List<long>();

            SimplePriorityQueue<long, long> forward = new SimplePriorityQueue<long, long>();
            SimplePriorityQueue<long, long> backward = new SimplePriorityQueue<long, long>();
            for (int i = 0; i < NodeCount; i++)
            {
                forward.Enqueue(i + 1, dist[i]);
                backward.Enqueue(i + 1, distR[i]);

            }
            do
            {
                long v = forward.Dequeue();

                Process(forward, v, dist,proc,  Graph, Weight);

                if (procR.Contains(v))
                    return ShortestPath(s, dist, t, distR);
                long vR = backward.Dequeue();
                Process(backward, vR, distR, procR, GraphR, WeightR);

                if (proc.Contains(vR))
                    return ShortestPath(t, distR,  s, dist);

            } while (true);


        }

        public long ShortestPath(long s, long[] dist,  
            long t, long[] distR)
        {
            long distance = int.MaxValue;
            long uBest = -1;
            for (int i = 0; i < NodeCount; i++)
            {

                if (dist[i] + distR[i] < distance)
                {
                    uBest = i + 1;
                    distance = dist[i] + distR[i];
                }
  }
            return distance;

        }
 private void Process(SimplePriorityQueue<long, long> q, long u, long[] dist, List<long> proc, List<long>[] Graph, long[,] Weight)
        {
            foreach (var v in Graph[u - 1])
            {
               if (dist[v-1] > dist[u-1] + Weight[u-1, v-1])
                {
                    dist[v-1] = dist[u-1] + Weight[u-1, v-1];
                   q.Enqueue(v , dist[v-1]);
                }

            }
            proc.Add(u);
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