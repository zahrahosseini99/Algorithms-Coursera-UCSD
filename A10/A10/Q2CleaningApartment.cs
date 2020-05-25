using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q2CleaningApartment : Processor
    {
        public Q2CleaningApartment(string testDataName) : base(testDataName)
        { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public String[] Solve(int V, int E, long[,] matrix)
        {

            long[,] table = FillingTable(matrix, V, E);

            List<string> clauses = new List<string>();

            List<long> tempList = new List<long>();

            //Each Node most appear in the path
            CheckingEachNodeExistence(clauses, table, V);
            //no two nodes appearing in a same position
            CheckingForNodesPositions(clauses, table, V);
            //Nonadjacent nodes i and j cannot be adjacent in the path
            CheckingForAdjacentNodes(matrix, clauses, table, V, E);


            String[] res = new string[clauses.Count + 1];
            res[0] = $"{clauses.Count} {V * V}";
            for (int i = 0; i < clauses.Count; i++)
            {
                res[i + 1] = clauses[i];
            }
            return res;
        }

        private void CheckingForAdjacentNodes(long[,] matrix, List<string> clauses, long[,] table, int v, int e)
        {
            long[,] adj = CounstructAdjMatrix(v, e, matrix);
            for (int i = 0; i < v; i++)
            {
                for (int j = i + 1; j < v; j++)
                {
                    if (adj[i, j] == 0)
                    {
                        for (int k = 0; k < v - 1; k++)
                        {
                            //X-ki ^ X-k+1 j ==  X-ki' | X-k+1' for all (i,j) is'nt a member of G and k = 1; 2,..., n-1
                            clauses.Add($"-{table[i, k]} -{table[j, k + 1]} 0");
                            clauses.Add($"-{table[j, k]} -{table[i, k + 1]} 0");
                        }
                    }

                }

            }

        }

        private void CheckingForNodesPositions(List<string> clauses, long[,] table, int v)
        {

            for (int i = 0; i < v; i++)
            {
               
                long[] temp = getColumn(table, i, v);
                // 1)one of the vertices should appear in a position i so--> OR all nodes <--Every position i on the path must be occupied
                clauses.Add(string.Join(" ", temp) + " 0");
                for (int j = 0; j < v; j++)
                {
                    for (int k = j + 1; k < v; k++)
                    {
                        //2)for other clauses X-ij ^ X-ik == 0 --> cnf= NOT(X-ij ^ X-ik)= X-ij' | X-ik'
                        clauses.Add($"-{temp[j]} -{temp[k]} 0");
                    }

                }

            }
        }
        private long[] getColumn(long[,] table, int i, int v)
        {
            var res = new long[v];
            for (int j = 0; j < v; j++)
            {
                res[j] = table[j, i];
            }
            return res;
        }
        private void CheckingEachNodeExistence(List<string> clauses, long[,] table, int v)
        {
            for (int i = 0; i < v; i++)
            {
                //1)all of vertices should be traversed
                long[] temp = getRow(table, i, v);
                clauses.Add(string.Join(" ", temp) + " 0");
                for (int j = 0; j < v; j++)
                {
                    for (int k = j + 1; k < v; k++)
                        //2)No node j appears twice in the path
                        clauses.Add($"-{table[i, j]} -{table[i, k]} 0");
                }

            }
        }

        private long[] getRow(long[,] table, int a, int v)
        {
            long[] res = new long[v];
            for (int i = 0; i < v; i++)
            {
                res[i] = table[a, i];
            }
            return res;
        }

        public long[,] GetVarNums(int v)
        {
            long[,] variables = new long[v, v];
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                    variables[i, j] = i * v + j + 1;
            }

            return variables;
        }

        public long[,] CounstructAdjMatrix(int v, int e, long[,] matrix)
        {
            long[,] adj = new long[v, v];
            for (int i = 0; i < e; i++)
            {
                adj[matrix[i, 0] - 1, matrix[i, 1] - 1] = 1;
                adj[matrix[i, 1] - 1, matrix[i, 0] - 1] = 1;
            }
            return adj;
        }
        private long[,] FillingTable(long[,] matrix, int v, int e)
        {
            long[,] table = new long[v, v];
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                    table[i, j] = GetVarNum(i  ,v ,j);
            }

            return table;
        }

        private long GetVarNum(int i, int v, int j) => i * v + j + 1;
    }
}
