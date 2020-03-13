using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using static A4.Q1BuildingRoads;

namespace A4
{
    public class Q2Clustering : Processor
    {
        public Q2Clustering(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, double>)Solve);

        public double Solve(long pointCount, long[][] points, long clusterCount)
        {
            Node[] graph = new Node[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                graph[i] = new Node(points[i][0], points[i][1], i);
            }

            double[] res = Prim(graph).ToArray();
            QuickSort(res, 0, res.Length - 1);

            return Math.Round(res[res.Length - clusterCount + 1], 6);
        }
        static int Partition(double[] arr, int low,
                                  int high)
        {
            double pivot = arr[high];
            int i = (low - 1);
            for (int j = low; j < high; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    double temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            double temp1 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp1;
            return i + 1;
        }

        static void QuickSort(double[] arr, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(arr, low, high);
                QuickSort(arr, low, pi - 1);
                QuickSort(arr, pi + 1, high);
            }
        }

        public List<double> Prim(Node[] graph)
        {
            double[] cost = new double[graph.Length];
            bool[] proc = new bool[graph.Length];
            for (int i = 0; i < graph.Length; i++)
            {
                cost[i] = double.PositiveInfinity;
                proc[i] = false;
            }
            cost[0] = 0;
            List<double> res = new List<double>();
            long count = graph.Length;
            do
            {
                long v = DequeueMin(cost, proc);
                proc[v - 1] = true;
                res.Add(cost[v - 1]);
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
            } while (count != 0);

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
}
