using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;
using TestCommon;

namespace A11
{
    public class Q4RescheduleExam : Processor
    {
        public Q4RescheduleExam(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, char[], long[][], char[]>)Solve);

        public override Action<string, string> Verifier =>
            TestTools.GraphColorVerifier;
        public bool[] visited;
        public long[] res;
        List<List<long>> allScc;
        public long[] cc;

        public virtual char[] Solve(long nodeCount, char[] colors, long[][] edges)
        {
            List<long[]> clauses = new List<long[]>();

            CheckingVertics(nodeCount, clauses, colors);

            CheckingEdges(clauses, colors, edges);

            Tuple<bool, long[]> result = Solve(3 * nodeCount, clauses.Count, clauses.ToArray());
            if (!result.Item1 && result.Item2 == null)
                return "Impossible".ToCharArray();

            char[] newColors = new char[nodeCount];
            findNewColor(newColors, nodeCount, result);

            return newColors;
        }

        private void findNewColor(char[] newColors, long nodeCount, Tuple<bool, long[]> result)
        {
            foreach (var key in result.Item2)
            {
                if (key < 0)
                    continue;
                else
                    assignNewColor(key, newColors);
            }
        }

        private void assignNewColor(long key, char[] newColors)
        {
            long node = (key - 1) / 3;
            long index = key % 3;
            if (index == 0)
            {
                newColors[node] = 'B';
            }
            else if (index == 1)
            {
                newColors[node] = 'G';
            }
            else
                newColors[node] = 'R';

        }

        private void CheckingEdges(List<long[]> clauses, char[] colors, long[][] edges)
        {
            foreach (var e in edges)
            {
                long left = e[0] * 3 - 2;
                long right = e[1] * 3 - 2;

                clauses.Add(new long[] { -1 * left, -1 * right });
                clauses.Add(new long[] { -1 * (left + 1), -1 * (right + 1) });
                clauses.Add(new long[] { -1 * (left + 2), -1 * (right + 2) });
            }
        }

        private void CheckingVertics(long nodeCount, List<long[]> clauses, char[] colors)
        {
            //RGBA
            for (int i = 0; i < nodeCount; i++)
            {
                if (colors[i] == 'R')
                {
                    clauses.Add(new long[] { getVarNum(i, 'B'), getVarNum(i, 'G') });
                    clauses.Add(new long[] { -1 * getVarNum(i, 'B'), -1 * getVarNum(i, 'G') });
                    clauses.Add(new long[] { -1 * getVarNum(i, 'R'), -1 * getVarNum(i, 'R') });
                }
                else if (colors[i] == 'G')
                {
                    clauses.Add(new long[] { getVarNum(i, 'B'), getVarNum(i, 'R') });
                    clauses.Add(new long[] { -1 * getVarNum(i, 'B'), -1 * getVarNum(i, 'R') });
                    clauses.Add(new long[] { -1 * getVarNum(i, 'G'), -1 * getVarNum(i, 'G') });
                }
                else
                {
                    clauses.Add(new long[] { getVarNum(i, 'G'), getVarNum(i, 'R') });
                    clauses.Add(new long[] { -1 * getVarNum(i, 'G'), -1 * getVarNum(i, 'R') });
                    clauses.Add(new long[] { -1 * getVarNum(i, 'B'), -1 * getVarNum(i, 'B') });
                }
            }

        }

        private long getVarNum(int i, char c)
        {
            if (c == 'R')
                return 3 * i + 1;
            else if (c == 'G')
                return 3 * i + 2;
            else
                return 3 * i + 3;
        }


        public virtual Tuple<bool, long[]> Solve(long v, long c, long[][] cnf)
        {
            res = new long[2 * v];
            visited = new bool[2 * v];
            cc = new long[2 * v];
            allScc = new List<List<long>>();

            List<long>[] graph = new List<long>[2 * v];
            List<long>[] reversegraph = new List<long>[2 * v];
            constructGraph(graph, reversegraph, cnf, v, c);

            long sccCount = SCC(2 * v, graph, reversegraph, allScc);
            if (!checkingScc(allScc, v))
                return new Tuple<bool, long[]>(false, null);

            long[] temp = new long[2 * v];
            bool[] vis = Enumerable.Repeat(false, (int)(2 * v)).ToArray();
            checkingLiterals(temp, v, sccCount, vis);

            long[] result = new long[v];
            for (long i = 0; i < v; i++)
            {
                if (vis[2 * i] && temp[2 * i] == 1)
                    result[i] = i + 1;
                else
                    result[i] = -1 * (i + 1);
            }

            return new Tuple<bool, long[]>(true, result);
        }

        private void checkingLiterals(long[] temp, long v, long sccCount, bool[] vis)
        {


            for (int i = 0; i < sccCount; i++)
            {
                if (!vis[allScc[i][0]])
                {

                    for (int j = 0; j < allScc[i].Count; j++)
                    {
                        if (allScc[i][j] % 2 == 0)
                        {
                            temp[allScc[i][j]] = 1;
                            vis[allScc[i][j]] = true;
                            temp[allScc[i][j] + 1] = 0;
                            vis[allScc[i][j] + 1] = true;
                        }
                        else
                        {
                            temp[allScc[i][j]] = 1;
                            vis[allScc[i][j]] = true;
                            temp[allScc[i][j] - 1] = 0;
                            vis[allScc[i][j] - 1] = true;
                        }
                    }
                }

            }
        }

        private bool checkingScc(List<List<long>> allScc, long v)
        {
            for (int i = 1; i <= v; i++)
            {
                if (cc[indexing(i)] == cc[indexing(-i)])
                    return false;
            }
            return true;
        }

        private void constructGraph(List<long>[] graph, List<long>[] reversegraph, long[][] cnf, long v, long c)
        {
            for (int i = 0; i < 2 * v; i++)
            {
                graph[i] = new List<long>();
                reversegraph[i] = new List<long>();
            }

            foreach (var g in cnf)
            {
                graph[indexing(-1 * g[0])].Add(indexing(g[1]));
                graph[indexing(-1 * g[1])].Add(indexing(g[0]));

                reversegraph[indexing(g[1])].Add(indexing(-1 * g[0]));
                reversegraph[indexing(g[0])].Add(indexing(-1 * g[1]));
            }
        }

        private long indexing(long v)
        {
            if (v < 0) return -2 * v - 1;
            else return 2 * v - 2;
        }

        public long SCC(long nodeCount, List<long>[] g, List<long>[] gr, List<List<long>> allScc)
        {
            long sccCount = 0;
            bool[] vist = new bool[nodeCount];

            ReversePost(vist, nodeCount, gr);
            List<long> temp = new List<long>();
            foreach (var w in res)
            {
                if (!visited[w])
                {

                    temp = new List<long>();
                    Explore(w, g, temp);
                    allScc.Add(temp);
                    sccCount++;
                }
            }
            return sccCount;
        }
        public void Explore(long v, List<long>[] g, List<long> temp)
        {
            Stack<long> explore = new Stack<long>();
            explore.Push(v);
            long explored = 0;
            while (explore.Any())
            {
                explored = explore.Pop();
                temp.Add(explored);
                cc[explored] = allScc.Count;
                visited[explored] = true;
                foreach (var w in g[explored])
                    if (!visited[w])
                        explore.Push(w);
            }

        }
        public void ReversePost(bool[] vist, long nodeCount, List<long>[] g)
        {
            Stack<long> top = new Stack<long>();

            for (int i = 0; i < nodeCount; i++)
            {
                if (vist[i] == false)
                {
                    Post(i, vist, top, g);
                }
            }
            int j = 0;
            while (top.Count != 0)
            {
                res[j++] = top.Pop();
            }

        }

        private void Post(long v, bool[] visited, Stack<long> top, List<long>[] g)
        {
            visited[v] = true;
            for (int j = 0; j < g[v].Count; j++)
            {
                if (!visited[g[v][j]])
                    Post(g[v][j], visited, top, g);
            }
            top.Push(v);
        }
    }
}
