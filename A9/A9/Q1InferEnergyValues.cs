using System;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public Q1InferEnergyValues(string testDataName) : base(testDataName)
        {
   
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            return GaussianElimination(MATRIX_SIZE, matrix);
           
        }

        private double[] GaussianElimination(long n, double[,] matrix)
        {
            ChangeMatrix(n, matrix);
            return ComputeVariables(n, matrix);
        }

        private double[] ComputeVariables(long n, double[,] matrix)
        {
            double[] result = new double[matrix.GetLongLength(1) - 1];
            for (long i = n - 1; i >= 0; i--)
            {
                result[i] = matrix[i, matrix.GetLongLength(1) - 1];
                for (long j = i + 1; j < n; j++)
                {
                    result[i] -= matrix[i, j] * result[j];
                }
                result[i] = result[i] / matrix[i, i];


            }
            for (int i = 0; i < result.Length; i++)
            {
                
                Rounding(result, result[i], i);
                if (result[i] == -0)
                    result[i] = 0;
            }
            return result;
        }

        private void Rounding(double[] result, double rslt, long i)
        {
            if (rslt > 0)
            {
                var k = Math.Floor(rslt);
                var x = Math.Ceiling(rslt);
                if (result[i] - k < 0.25)
                    result[i] = k;
                else if (result[i] - k > 0.75)
                    result[i] = x;
                else
                    result[i] = k + 0.5;
            }
            else if (rslt == 0)
            {
                result[i] = 0;
            }
            else
            {
                var k = Math.Floor(rslt);
                var x = Math.Ceiling(rslt);
                var r = Math.Abs(result[i] + Math.Abs(x));
                if (r < 0.25)
                    result[i] = x;
                else if (r > 0.75)
                    result[i] = k;
                else
                    result[i] = Math.Ceiling(rslt) - 0.5;
            }

        }

        private void ChangeMatrix(long n, double[,] matrix)
        {
            for (long k = 0; k < n; k++)
            {
                long i_max = k;
                double v_max = matrix[i_max, k];
                for (long i = k + 1; i < n; i++)
                {
                    if (Math.Abs(matrix[i, k]) > Math.Abs(v_max))
                    {
                        v_max = matrix[i, k];
                        i_max = i;
                    }
                }
                if (i_max != k)
                    SwapRow(matrix, n, k, i_max);
                for (long i = k + 1; i < n; i++)
                {
                    if (matrix[k, k] == 0)
                    {
                        SwapRow(matrix, n, k, i_max);

                    }


                    double f = matrix[i, k] / matrix[k, k];

                    for (long j = k + 1; j < matrix.GetLongLength(1); j++)
                    {
                        matrix[i, j] -= matrix[k, j] * f;

                    }
                    matrix[i, k] = 0;
                }

            }

        }

        private void SwapRow(double[,] matrix, long n, long i, long j)
        {
            for (long k = 0; k < matrix.GetLongLength(1); k++)
            {
                var tmp = matrix[i, k];
                matrix[i, k] = matrix[j, k];
                matrix[j, k] = tmp;
            }
        }
    }
}
