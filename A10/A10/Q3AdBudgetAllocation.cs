using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q3AdBudgetAllocation : Processor
    {
        public Q3AdBudgetAllocation(string testDataName) : base(testDataName) {  }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public string[] Solve(long eqCount, long varCount, long[][] A, long[] b)
        {
            //at all have 14=8+4+2=2^3+2^2+2^1
            List<string> clauses = new List<string>();
            for (int i = 0; i < A.Length; i++)
            {
                long NonZeroCoe = 0;
                List<long> nonZero = new List<long>();
                for (int j = 0; j < A[i].Length; j++)
                {
                    if (A[i][j] != 0)
                    {
                        NonZeroCoe = NonZeroCoe + 1;
                        nonZero.Add(j);
                    }

                }
                if (NonZeroCoe == 0)
                    continue;
                if (NonZeroCoe == 3)
                    CheckingThreeVar(clauses, A, i, nonZero, eqCount, varCount, b);
                else if (NonZeroCoe == 2)
                    CheckinfTwoVar(clauses, A, i, nonZero, eqCount, varCount, b);
                else
                    CheckingOneVar(clauses, A, i, nonZero, eqCount, varCount, b);

            }
            String[] res = new string[clauses.Count + 1];
            res[0] = $"{clauses.Count} {varCount}";
            for (int i = 0; i < clauses.Count; i++)
            {
                res[i + 1] = clauses[i];
            }
            return res;

        }

        private void CheckingOneVar(List<string> clauses, long[][] a, int index, List<long> nonZero, long eqCount, long varCount, long[] b)
        {
            List<long> temp;
            //0
            if (a[index][nonZero[0]] * 0 > b[index])
            {
                temp = new List<long>();
                temp.Add(nonZero[0] + 1);
                clauses.Add(string.Join(" ", temp) + " 0");
            }
            if (a[index][nonZero[0]] * 1 > b[index])
            {
                temp = new List<long>();
                temp.Add((-1) * (nonZero[0] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");
            }
        }

        private void CheckinfTwoVar(List<string> clauses, long[][] a, int index, List<long> nonZero, long eqCount, long varCount, long[] b)
        {
            List<long> temp;
            //00
            if (a[index][nonZero[0]] * 0 + a[index][nonZero[1]] * 0 > b[index])
            {
                temp = new List<long>();
                temp.Add(nonZero[0] + 1);
                temp.Add(nonZero[1] + 1);
                clauses.Add(string.Join(" ", temp) + " 0");
            }
            //01
            if (a[index][nonZero[0]] * 0 + a[index][nonZero[1]] * 1 > b[index])
            {
                temp = new List<long>();
                temp.Add(nonZero[0] + 1);
                temp.Add((-1) * (nonZero[1] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");
            }
            //10
            if (a[index][nonZero[0]] * 1 + a[index][nonZero[1]] * 0 > b[index])
            {
                temp = new List<long>();
                temp.Add((-1) * (nonZero[0] + 1));
                temp.Add((nonZero[1] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");
            }
            //11
            if (a[index][nonZero[0]] * 1 + a[index][nonZero[1]] * 1 > b[index])
            {
                temp = new List<long>();
                temp.Add((-1) * (nonZero[0] + 1));
                temp.Add((-1) * (nonZero[1] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");
            }
        }

        private void CheckingThreeVar(List<string> clauses, long[][] a, int index, List<long> nonZero, long eqCount, long varCount, long[] b)
        {


            List<long> temp;


            //000
            if (a[index][nonZero[0]] * 0 + a[index][nonZero[1]] * 0 + a[index][nonZero[2]] * 0 > b[index])
            {
                temp = new List<long>();
                temp.Add(nonZero[0] + 1);
                temp.Add(nonZero[1] + 1);
                temp.Add(nonZero[2] + 1);
                clauses.Add(string.Join(" ", temp) + " 0");
            }

            //001
            if (a[index][nonZero[0]] * 0 + a[index][nonZero[1]] * 0 + a[index][nonZero[2]] * 1 > b[index])
            {
                temp = new List<long>();
                temp.Add((nonZero[0] + 1));
                temp.Add((nonZero[1] + 1));
                temp.Add((-1) * (nonZero[2] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");

            }
            //010
            if (a[index][nonZero[0]] * 0 + a[index][nonZero[1]] * 1 + a[index][nonZero[2]] * 0 > b[index])
            {
                temp = new List<long>();
                temp.Add((nonZero[0] + 1));
                temp.Add((-1) * (nonZero[1] + 1));
                temp.Add((nonZero[2] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");

            }
            //011
            if (a[index][nonZero[0]] * 0 + a[index][nonZero[1]] * 1 + a[index][nonZero[2]] * 1 > b[index])
            {
                temp = new List<long>();
                temp.Add((nonZero[0] + 1));
                temp.Add((-1) * (nonZero[1] + 1));
                temp.Add((-1) * (nonZero[2] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");

            }
            //100
            if (a[index][nonZero[0]] * 1 + a[index][nonZero[1]] * 0 + a[index][nonZero[2]] * 0 > b[index])
            {
                temp = new List<long>();
                temp.Add((-1) * (nonZero[0] + 1));
                temp.Add(nonZero[1] + 1);
                temp.Add(nonZero[2] + 1);
                clauses.Add(string.Join(" ", temp) + " 0");

            }
            //101
            if (a[index][nonZero[0]] * 1 + a[index][nonZero[1]] * 0 + a[index][nonZero[2]] * 1 > b[index])
            {
                temp = new List<long>();
                temp.Add((-1) * (nonZero[0] + 1));
                temp.Add(nonZero[1] + 1);
                temp.Add((-1) * (nonZero[2] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");

            }
            //110
            if (a[index][nonZero[0]] * 1 + a[index][nonZero[1]] * 1 + a[index][nonZero[2]] * 0 > b[index])
            {
                temp = new List<long>();
                temp.Add((-1) * (nonZero[0] + 1));
                temp.Add((-1) * (nonZero[1] + 1));
                temp.Add((nonZero[2] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");

            }
            ///111
            if (a[index][nonZero[0]] * 1 + a[index][nonZero[1]] * 1 + a[index][nonZero[2]] * 1 > b[index])
            {
                temp = new List<long>();
                temp.Add((-1) * (nonZero[0] + 1));
                temp.Add((-1) * (nonZero[1] + 1));
                temp.Add((-1) * (nonZero[2] + 1));
                clauses.Add(string.Join(" ", temp) + " 0");

            }

        }

        private List<long> getCeo(long[][] a, int i)
        {
            List<long> res = new List<long>();
            for (int j = 0; j < a[i].Length; j++)
            {
                if (a[i][j] != 0)
                    res.Add(a[i][j]);
            }
            return res;
        }
    }
}
