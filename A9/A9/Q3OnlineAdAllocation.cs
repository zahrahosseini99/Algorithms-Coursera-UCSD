using System;
using System.Text;
using TestCommon;

namespace A9
{
    public class Q3OnlineAdAllocation : Processor
    {

        public Q3OnlineAdAllocation(string testDataName) : base(testDataName)
        {

        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int c, int v, double[,] matrix1)
        {
            double[,] table = new double[c + 1, v + c + 1];

            ConstructSimplexTable(matrix1, v, c, table);


            return SImplexMethod(table, c, v, matrix1);
        }

        private string SImplexMethod(double[,] table, int c, int v, double[,] matrix1)
        {
            int Entering;
            while (ThereIsNegative(table, out Entering))
            {
                int pivot = FindPivotRow(table, Entering);
                if (pivot == -1)
                    return "Infinity";
                changePivotRow(table, Entering, pivot);
                changeTable(table, Entering, pivot);
            }

            var str = new StringBuilder();
            int index;
            string res = "";
            for (int i = 0; i < v; i++)
            {
                if (checkColumn(table, i, out index))
                {
                    table[index, table.GetLength(1) - 1] = Rounding(table[index, table.GetLength(1) - 1]);
                    res += table[index, table.GetLength(1) - 1].ToString() + " ";
                }
                else
                    res += 0.ToString() + " ";
            }

            double[] resu = new double[v];
            bool flag;
            for (int i = 0; i < v; i++)
            {
                flag = false;

                for (int j = 0; j < c; j++)
                {
                    if (table[j, i] == 1 && !flag)
                    {
                        resu[i] = table[j, table.GetLength(1) - 1];
                        flag = true;
                    }
                    else if (table[j, i] != 0 && flag)
                    {
                        resu[i] = 0;
                        break;
                    }
                }
            }

            double temp;
            for (int i = 0; i < matrix1.GetLength(0) - 1; i++)
            {
                temp = 0;
                for (int j = 0; j < matrix1.GetLength(1) - 1; j++)
                {
                    temp += matrix1[i, j] * Math.Round(resu[j] - 0.1);
                }
                if (temp > matrix1[i, matrix1.GetLength(1) - 1])
                    return "No Solution";
            }

            if (res != "")
                str.Append("Bounded Solution" + "\n");
            str.AppendLine(res);
            return str.ToString();
        }

        private void ConstructSimplexTable(double[,] matrix1, int v, int c, double[,] table)
        {
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1) - 1; j++)
                {
                    table[i, j] = matrix1[i, j];
                    if (i == matrix1.GetLength(0) - 1)
                        table[i, j] = (-1) * matrix1[i, j];
                }
                int count = 0;
                int k = matrix1.GetLength(1) - 1;
                while (k < table.GetLength(1) - 1)
                {
                    if (count == i)
                        table[i, k] = 1;
                    else
                        table[i, k] = 0;
                    k++;
                    count++;
                }

                table[i, table.GetLength(1) - 1] = matrix1[i, matrix1.GetLength(1) - 1];
            }
        }

        private double Rounding(double rslt)
        {
            if (rslt > 0)
            {
                var k = Math.Floor(rslt);
                var x = Math.Ceiling(rslt);
                if (rslt - k < 0.25)
                    rslt = k;
                else if (rslt - k > 0.75)
                    rslt = x;
                else
                    rslt = k + 0.5;
            }
            else if (rslt == 0)
            {
                rslt = 0;
            }
            else
            {
                var k = Math.Floor(rslt);
                var x = Math.Ceiling(rslt);
                var r = Math.Abs(rslt + Math.Abs(x));
                if (r < 0.25)
                    rslt = x;
                else if (r > 0.75)
                    rslt = k;
                else
                    rslt = Math.Ceiling(rslt) - 0.5;
            }
            return rslt;
        }
        private bool checkColumn(double[,] table, int j, out int index)
        {

            int count = 0;
            index = -1;
            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (table[i, j] != 0)
                {
                    if (table[i, j] != 1)
                    {

                        return false;
                    }

                    count++;
                    index = i;
                }

            }
            if (count > 1)
            {
                return false;

            }
            else
                return true;
        }

        private void changeTable(double[,] table, int entering, int pivot)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (table[i, entering] != 0 && i != pivot)
                {
                    double x = table[i, entering] / table[pivot, entering];
                    ChangeRow(x, table, i, pivot);
                }
            }
        }

        private void ChangeRow(double x, double[,] table, int index, int pivot)
        {
            for (int i = 0; i < table.GetLength(1); i++)
            {
                table[index, i] = table[index, i] - (x * table[pivot, i]);
            }
        }

        private void changePivotRow(double[,] table, int entering, int pivot)
        {
            double p = table[pivot, entering];
            for (int i = 0; i < table.GetLength(1); i++)
            {
                table[pivot, i] = table[pivot, i] / p;
            }
        }

        private int FindPivotRow(double[,] table, int Entering)
        {
            double pivot = int.MaxValue;
            int index = -1;
            for (int i = 0; i < table.GetLength(0) - 1; i++)
            {
                if (table[i, Entering] > 0)
                {
                    if (pivot > table[i, table.GetLength(1) - 1] / table[i, Entering])
                    {
                        pivot = table[i, table.GetLength(1) - 1] / table[i, Entering];
                        index = i;
                    }
                }

            }
            return index;
        }

        private bool ThereIsNegative(double[,] table, out int Entering)
        {
            bool res = false;
            Entering = -1;
            double cmp = double.MaxValue;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (table[table.GetLength(0) - 1, i] < 0)
                {
                    res = true;
                    if (cmp > table[table.GetLength(0) - 1, i])
                    {
                        Entering = i;
                        cmp = table[table.GetLength(0) - 1, i];
                    }

                }

            }
            return res;
        }
    }
}
