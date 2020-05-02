using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q2Outbreak : Processor
    {
        public Q2Outbreak(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<string[], string>)Solve);

        public static Tuple<int, int, int[,], int[,]> ProcessQ2(string[] data)
        {
            var temp = data[0].Split();
            int N = int.Parse(temp[0]);
            int M = int.Parse(temp[1]);
            int[,] carriers = new int[N, 2];
            int[,] safe = new int[M, 2];
            for (int i = 0; i < N; i++)
            {
                carriers[i, 0] = int.Parse(data[i + 1].Split()[0]);
                carriers[i, 1] = int.Parse(data[i + 1].Split()[1]);
            }

            for (int i = 0; i < M; i++)
            {
                safe[i, 0] = int.Parse(data[i + N + 1].Split()[0]);
                safe[i, 1] = int.Parse(data[i + N + 1].Split()[1]);
            }
            return Tuple.Create(N, M, carriers, safe);
        }
        public string Solve(string[] input)
        {
            var data = ProcessQ2(input);
            return Solve(data.Item1,data.Item2,data.Item3,data.Item4).ToString();
        }
        public double Solve(int N, int M, int[,] carrier, int[,] safe)
        {
            Node[] graph = new Node[carrier.Length+safe.Length];
            for (int i = 0; i < carrier.Length-1; i++)
            {
                graph[i] = new Node(carrier[i,0], carrier[i,1], i);
            }
            int count = carrier.Length;
            for (int i = 0; i < safe.Length-1; i++)
            {
                graph[i+count] = new Node(safe[i, 0], safe[i, 1], i);
               // count++;
            }
            double[] res = Prim(graph).ToArray();
            QuickSort(res, 0, res.Length - 1);

            return Math.Round(res[res.Length -1], 6);
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
        public class Node
        {
            public double Height;
            public double Length;
            public long index;
            public Node(double length, double height, long i)
            {
                Length = length;
                Height = height;
                index = i;
            }

        }
    }
}
