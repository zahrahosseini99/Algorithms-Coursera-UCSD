using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using GeoCoordinatePortable;
using Priority_Queue;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public static readonly char[] IgnoreChars = new char[] { '\n', '\r', ' ' };
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        private static double[][] ReadTree(IEnumerable<string> lines)
        {
            return lines.Select(line =>
                line.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(n => double.Parse(n)).ToArray()
                            ).ToArray();
        }
        public override string Process(string inStr)
        {
            return Process(inStr, (Func<long, long, double[][], double[][], long,
                                    long[][], double[]>)Solve);
        }
        public static string Process(string inStr, Func<long, long, double[][]
                                  , double[][], long, long[][], double[]> processor)
        {
            var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            long[] count = lines.First().Split(IgnoreChars,
                                               StringSplitOptions.RemoveEmptyEntries)
                                         .Select(n => long.Parse(n))
                                         .ToArray();
            double[][] points = ReadTree(lines.Skip(1).Take((int)count[0]));
            double[][] edges = ReadTree(lines.Skip(1 + (int)count[0]).Take((int)count[1]));
            long queryCount = long.Parse(lines.Skip(1 + (int)count[0] + (int)count[1])
                                         .Take(1).FirstOrDefault());
            long[][] queries = ReadTree(lines.Skip(2 + (int)count[0] + (int)count[1]))
                                        .Select(x => x.Select(z => (long)z).ToArray())
                                        .ToArray();

            return string.Join("\n", processor(count[0], count[1], points, edges,
                                queryCount, queries));
        }
        public double[] Solve(long nodeCount,
                            long edgeCount,
                            double[][] points,
                            double[][] edges,
                            long queriesCount,
                            long[][] queries)
        {

            Graph g = new Graph(edges, nodeCount);
            double[] res = new double[queriesCount];

            int i = 0;
            foreach (var q in queries)
            {
                double result = 0;

                if (queries[i][0] == queries[i][1])
                    result = 0;
                else
                    result = AStar(nodeCount, g, points, queries[i][0], queries[i][1]);
                if (result == int.MaxValue)
                    res[i++] = -1;
                else
                    res[i++] = result;
            }

            return res;
        }

        private double AStar(long nodeCount, Graph g, double[][] points, long s, long t)
        {
            double[] dist = new double[nodeCount];
            double[] distR = new double[nodeCount];
            bool[] proc = new bool[nodeCount];
            List<long> Relaxed = new List<long>();
            SimplePriorityQueue<long> forward = new SimplePriorityQueue<long>();
            SimplePriorityQueue<long> backward = new SimplePriorityQueue<long>();

            for (long i = 0; i < nodeCount; i++)
            {
                dist[i] = long.MaxValue;
                distR[i] = long.MaxValue;
                proc[i] = false;
            }

            dist[s - 1] = 0;
            forward.Enqueue(s - 1, (float)(dist[s - 1] + Pi(points, t - 1, s - 1)));
            Relaxed.Add(s - 1);

            distR[t - 1] = 0;
            backward.Enqueue(t - 1, (float)(dist[t - 1] + Pi(points, s - 1, t - 1)));
            Relaxed.Add(t - 1);

            long v;
            long vR;

            do
            {
                v = forward.Dequeue();
                GraphProcess(points, v, g.G, dist, forward, Relaxed, t - 1);
                if (proc[v])
                    return ShortestPath(dist, distR, Relaxed);
                proc[v] = true;

                vR = backward.Dequeue();
                GraphProcess(points, vR, g.GraphR, distR, backward, Relaxed, s - 1);
                if (proc[vR])
                    return ShortestPath(dist, distR, Relaxed);
                proc[vR] = true;

            } while (forward.Count != 0 && backward.Count != 0);

            return -1;
        }

        private void GraphProcess(double[][] points, long v, List<(double, double)>[] g, double[] dist,
            SimplePriorityQueue<long> pq, List<long> Relaxed, long target)
        {
            foreach (var e in g[v])
            {
                if (dist[(long)e.Item1 - 1] > dist[(long)v] + e.Item2)
                {
                    if (dist[(long)v] + e.Item2 >= 0)
                    {
                        dist[(long)e.Item1 - 1] = dist[(long)v] + e.Item2;
                        if (pq.Contains((long)e.Item1 - 1))
                            pq.UpdatePriority((long)e.Item1 - 1, (float)(dist[(long)e.Item1 - 1] + Pi(points, (long)e.Item1 - 1, target)));
                        else
                            pq.Enqueue((long)e.Item1 - 1, (float)(dist[(long)e.Item1 - 1] + Pi(points, (long)e.Item1 - 1, target)));
                        Relaxed.Add((long)e.Item1 - 1);
                    }
                }
            }

        }

        public double Pi(double[][] points, long u, long v)
        {
            if ((int)(points[0][0]) == points[0][0])
            {
                return Math.Sqrt(Math.Pow(points[u][0] - points[v][0], 2) + Math.Pow(points[u][1] - points[v][1], 2));
            }
            else
            {
                GeoCoordinate start = new GeoCoordinate(points[u][1], points[u][0]);
                GeoCoordinate target = new GeoCoordinate(points[v][1], points[v][0]);
                return start.GetDistanceTo(target);
            }
        }

        public double ShortestPath(double[] dist, double[] distR, List<long> relaxed)
        {
            double distance = double.PositiveInfinity;
            double uBest = -1;

            foreach (var v in relaxed)
            {
                if (dist[v] + distR[v] < distance)
                {
                    uBest = v;
                    distance = dist[v] + distR[v];
                }
            }
            return distance;

        }

        public class Graph
        {

            public double[][] Edges;
            public long NodeCount;
            public List<(double, double)>[] G;
            public List<(double, double)>[] GraphR;
            public Graph(double[][] edges, long nodecount)
            {
                Edges = edges;
                NodeCount = nodecount;
                G = new List<(double, double)>[nodecount];
                GraphR = new List<(double, double)>[nodecount];

                for (int i = 0; i < nodecount; i++)
                {
                    G[i] = new List<(double, double)>();
                    GraphR[i] = new List<(double, double)>();
                }
                foreach (var g in edges)
                {
                    G[(int)g[0] - 1].Add((g[1], g[2]));
                    GraphR[(int)g[1] - 1].Add((g[0], g[2]));

                }

            }
        }
    }
}