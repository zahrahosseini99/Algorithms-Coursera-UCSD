using System.Collections.Generic;
using System.Linq;

namespace A1
{
    public class DirectedGraph
    {
        public long[][] Nodes;
        public long NodeCount;
        public List<long>[] Graph;
        public DirectedGraph(long[][] nodes, long nodecount)
        {
            Nodes = nodes;
            NodeCount = nodecount;
            Graph = new List<long>[nodecount];
            for (int i = 0; i < nodecount; i++)
            {
                Graph[i] = new List<long>();
            }
            foreach (var g in nodes)
            {
                Graph[g[0] - 1].Add(g[1]);
            }
        }
        public bool HasCycle(long v, List<long> First, List<long> Middle, List<long> Second)
        {
            Replace(v, First, Middle);
            for (int i = 0; i < Graph[v - 1].Count; i++)
            {
                if (Second.Contains(Graph[v - 1][i]))
                    continue;
                if (Middle.Contains(Graph[v - 1][i]))
                    return true;
                if (HasCycle(Graph[v - 1][i], First, Middle, Second))
                    return true;
            }
            Replace(v, Middle, Second);
            return false;
        }

        private void Replace(long v, List<long> source, List<long> target)
        {

            source.Remove(v);
            target.Add(v);

        }
        public void TopologicalSort(bool[] visited, long[] res)
        {
            Stack<long> top = new Stack<long>();

            for (int i = 0; i < NodeCount; i++)
            {
                if (visited[i] == false)
                {
                    PostOrder(i + 1, visited, top,Graph);
                }
            }
            int j = 0;
            while (top.Count != 0)
            {
                res[j++] = top.Pop();
            }

        }

        public void PostOrder(long v, bool[] visited, Stack<long> top, List<long>[] g)
        {
            visited[v - 1] = true;
            for (int j = 0; j < Graph[v - 1].Count; j++)
            {
                if (!visited[Graph[v - 1][j] - 1])
                    PostOrder(Graph[v - 1][j], visited, top,g);
            }
            top.Push(v);
        }
        public List<long>[] ReverseGraph()
        {
            List<long>[] graph = new List<long>[NodeCount];
            for (int i = 0; i < NodeCount; i++)
            {
                graph[i] = new List<long>();
            }
            foreach (var g in Nodes)
            {
                graph[g[1] - 1].Add(g[0]);
            }
            return graph;
        }
       
       
    }
}