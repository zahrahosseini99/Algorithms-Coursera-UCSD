using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q3ExchangingMoney : Processor
    {
        public Q3ExchangingMoney(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, string[]>)Solve);

        public string[] Solve(long nodeCount, long[][] edges, long startNode)
        {
            string[] res = new string[nodeCount];
            res[startNode-1] = "0";
            DirectedGraph graph = new DirectedGraph(edges, nodeCount);
            long[] dist = new long[nodeCount];

            for (long i = 0; i < nodeCount; i++)
                dist[i] = int.MaxValue;

            bool cycle=graph.HasNegativeCycle(startNode-1,dist);
            //var n = graph.Graph[startNode - 1].Count;
            //if (n==0 )
            //{
            //    cycle = !cycle;
            //}
            //if (n == 1)
            //{
            //    var m = graph.Graph[edges[startNode - 1][1] - 1].Count;
            //    if(m==0)
            //        cycle = !cycle;
            //}
            if (!cycle)
            {


                for (int i = 0; i < nodeCount; i++)
                {
                    if (dist[i] ==int.MaxValue)
                        res[i] = "*";
                    else
                        res[i] = dist[i].ToString();
                }

               
            }
            else
            {
                for (int i = 0; i < nodeCount; i++)
                {
                    if (dist[i] == int.MaxValue)
                        res[i] = "*";
                    else
                        res[i] = dist[i].ToString();
                }
                for (int i = 0; i < nodeCount; i++)
                {

                   

                    foreach (long[] e in edges)
                    {
                        if ( dist[e[1] - 1] > dist[e[0] - 1] + e[2])
                        {
                            if(res[e[1] - 1]!="*")
                            res[e[1] - 1] = "-";
                            dist[e[1] - 1] = dist[e[0] - 1] + e[2];
                        }
                    }


                   

                }
            }
            return res;
        }
    }
}
