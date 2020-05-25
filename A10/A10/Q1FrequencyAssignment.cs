using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q1FrequencyAssignment : Processor
    {
        public Q1FrequencyAssignment(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);


        public String[] Solve(int V, int E, long[,] matrix)
        {
            List<string> clauses = new List<string>();
            long[] colors = new long[] { 1, 2, 3 };
           
            CheckingVertics(clauses, colors, V);
            
            CheckingEdges(clauses, colors, matrix, E);
            String[] res = new string[clauses.Count + 1];
            res[0] = $"{clauses.Count} {V * 3}";
            for (int i = 0; i < clauses.Count; i++)
            {
                res[i + 1] = clauses[i];
            }
            return res;
        }

        private void CheckingEdges(List<string> clauses, long[] colors, long[,] matrix, int e)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < colors.Length; j++)
                {
                    clauses.Add($"-{GetVArNum(matrix[i, 0] - 1, colors[j])} -{GetVArNum(matrix[i, 1] - 1, colors[j])} 0");
                }
            }
        }

        private void CheckingVertics(List<string> clauses, long[] colors, int v)
        {
            for (int i = 0; i < v; i++)
            {
                long[] temp = getColors(colors, i);
                clauses.Add(string.Join(" ", temp) + " 0");

                for (int j = 0; j < temp.Length; j++)
                    for (int k = j + 1; k < temp.Length; k++)
                        clauses.Add($"-{temp[j]} -{temp[k]} 0");

            }
        }

        private long[] getColors(long[] colors, int i)
        {
            var res = new long[3];
            for (int j = 0; j < colors.Length; j++)
            {
                res[j] = GetVArNum(i, colors[j]);
            }
            return res;
        }

        private long GetVArNum(long v, long c) => 3 * v + c;

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

    }
}
