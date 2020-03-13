using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q1BuildingRoads : Processor
    {
        public Q1BuildingRoads(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], double>)Solve);

        public double Solve(long pointCount, long[][] points)
        {
            Node[] graph = new Node[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                graph[i] = new Node(points[i][0], points[i][1],i);
            }

            return Math.Round(Prim(graph), 6);
        }

        public double Prim(Node[] graph)
        {
            double[] cost = new double[graph.Length];
            bool[] proc = new bool[graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                cost[i] = double.PositiveInfinity;
                proc[i] = false;
            }
            cost[0] = 0;
            double res = 0;
            long count = graph.Length;
            do
            {
                long v = DequeueMin(cost, proc);
                proc[v - 1] = true;
                res += cost[v - 1];
                for (int i = 0; i < graph.Length; i++)
                {
                    if (!proc[i])
                    {
                        if (cost[i] > Weight(graph, v, i))
                        {
                            cost[i] = Weight(graph, v, i);

                        }
                    }
                }
                count--;
            } while (count!=0);

            return res;
        }

        private double Weight(Node[] graph, long v, int i)
        {
            double x = Math.Pow((double)(graph[v - 1].Length - graph[i].Length), (double)2);
            double y = Math.Pow((double)(graph[v - 1].Height - graph[i].Height), (double)2);
            return Math.Sqrt(x + y);
        }

        private long DequeueMin(double[] cost, bool[] proc)
        {
            double temp = double.PositiveInfinity;
            long index = -1;

            for (int i = 0; i < cost.Length; i++)
            {
                if (cost[i] < temp && !proc[i])
                {
                    temp = cost[i];
                    index = i;
                }
            }
          
            return index + 1;
        }
    }

    public class Node
    {
        public double Height;
        public double Length;
        public long index;
        public Node(double length, double height,long i)
        {
            Length = length;
            Height = height;
            index = i;
        }

    }
}
