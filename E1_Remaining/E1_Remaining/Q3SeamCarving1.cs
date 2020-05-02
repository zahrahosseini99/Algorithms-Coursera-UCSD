using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TestCommon;


namespace Exam1
{
    public class Q3SeamCarving1 : Processor // Calculate Energy
    {
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        public Q3SeamCarving1(string testDataName) : base(testDataName) { }

        public override string Process(string inStr)
        {
            // Parse input file
            string[] everyLine = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            int y = everyLine.Length;



            string[] temp = everyLine[0].Split('|');
            int x = temp.Length;

            Color[,] data = new Color[y, x];

            for (int i = 0; i < everyLine.Length; i++)
            {
                string[] lineWithoutLine = everyLine[i].Split('|');

                for (int j = 0; j < x; j++)
                {
                    string[] RGB = lineWithoutLine[j].Split(',');

                    Color tmp = Color.FromArgb(int.Parse(RGB[0]), int.Parse(RGB[1]), int.Parse(RGB[2]));

                    data[i, j] = tmp;
                }
            }
            var solved = Solve(data);
            // convert solved into output string
            string res = "";

            var r = new StringBuilder();
            for (int i = 0; i < solved.GetLength(0); i++)
            {
                res = "";
                for (int j = 0; j < solved.GetLength(1); j++)
                {
                    if (j != solved.GetLength(1) - 1)
                        res += solved[i, j] + ",";
                    else
                    {
                        res += solved[i, j];
                    }
                }

                r.AppendLine(res);
               
            }
            return r.ToString();
        }


        public double[,] Solve(Color[,] data)
        {

            double[,] res = new double[data.GetLength(0), data.GetLength(1)];

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if ((i == 0) || (j == 0) || (i == data.GetLength(0) - 1) || (j == data.GetLength(1) - 1))
                        res[i, j] = 1000;
                    else
                    {
                        double rX = (double)data[i + 1, j].A - (double)data[i - 1, j].A;
                        double rY = (double)data[i, j + 1].A - (double)data[i, j - 1].A;

                        double gX = (double)data[i + 1, j].G - (double)data[i - 1, j].G;
                        double gY = (double)data[i, j + 1].G - (double)data[i, j - 1].G;

                        double bX = (double)data[i + 1, j].B - (double)data[i - 1, j].B;
                        double bY = (double)data[i, j + 1].B - (double)data[i, j - 1].B;

                        double difX = (double)Math.Pow((double)rX, 2) + (double)Math.Pow((double)gX, 2) + (double)Math.Pow((double)bX, 2);
                        double difY = (double)Math.Pow((double)rY, 2) + (double)Math.Pow((double)gY, 2) + (double)Math.Pow((double)bY, 2);

                        double energy = Math.Sqrt((double)difX + (double)difY);
                        res[i, j] = Math.Round(energy, 3);
                    }

                }
            }
            return res;
        }
    }

    public class Q3SeamCarving2 : Processor // Find Seam
    {
        public Q3SeamCarving2(string testDataName) : base(testDataName) { }
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        public override string Process(string inStr)
        {
            // Parse input file

            string[] everyLine = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            int y = everyLine.Length;
            string[] temp = everyLine[0].Split(',');
            int x = temp.Length;
            double[,] data = new double[y, x];
            for (int i = 0; i < everyLine.Length; i++)
            {
                var tmp = everyLine[i].Split(',');
                for (int j = 0; j < x; j++)
                {
                    data[i, j] = double.Parse(tmp[j]);
                }
            }
            var solved = Solve(data);
            // convert solved into output string
            //عمودی افقی
            var r = new StringBuilder();
            string str = "";
            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (i != data.GetLength(0) - 1)
                    str += solved[i] + ",";
                else
                    str += solved[i];
            }
            r.AppendLine(str);
            str = "";
            for (int i = data.GetLength(0); i < data.GetLength(0) + data.GetLength(1); i++)
            {
                if (i != data.GetLength(0) + data.GetLength(1) - 1)
                    str += solved[i] + ",";
                else
                    str += solved[i];
            }
            r.AppendLine(str);
            return r.ToString();
        }


        public int[] Solve(double[,] data)
        {
            int[] res1 = new int[data.GetLength(0)];
            int[] res2 = new int[data.GetLength(1)];
            int[] result = new int[data.GetLength(0) + data.GetLength(1)];
            FindVerticalSeam(data, res1);
            FindHorizontalSeam(data, res2);
            int resiii = 0;
            resiii++;
            Array.Copy(res1, result, res1.Length);
            Array.Copy(res2, 0, result, res1.Length, res2.Length);

            return result;
        }

        private void FindHorizontalSeam(double[,] data, int[] res2)  //افقی
        {

            int indexFirst = 0;
            double eng = 1000;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (eng > data[i, 1])
                {
                    eng = data[i, 1];
                    indexFirst = i;
                }
            }
            res2[0] = indexFirst;
            res2[1] = indexFirst;
            int counter = 1;

            double prev = 0;

            while (counter < data.GetLength(1) - 1)
            {
                prev = data[indexFirst, counter];
                double energy = double.PositiveInfinity; ;
                for (int i = -1; i < 2; i++)
                {
                    if (prev + data[indexFirst + i, counter + 1] < energy)
                    {
                        energy = data[indexFirst, counter] + data[indexFirst + i, counter + 1];
                        res2[counter + 1] = indexFirst + i;
                    }

                }
                indexFirst = res2[counter + 1];
                counter++;
            }
            res2[data.GetLength(1) - 1] = res2[data.GetLength(1) - 2];
        }

        private void FindVerticalSeam(double[,] data, int[] res1)    //عمودی
        {

            int indexFirst = 0;
            double eng = 1000;
            for (int i = 0; i < data.GetLength(1); i++)
            {
                if (eng > data[1, i])
                {
                    eng = data[1, i];
                    indexFirst = i;
                }
            }
            res1[0] = indexFirst;
            res1[1] = indexFirst;
            int counter = 1;

            double prev = 0;

            while (counter < data.GetLength(0) - 1)
            {
                prev = data[counter, indexFirst];
                double energy = double.PositiveInfinity; ;
                for (int i = -1; i < 2; i++)
                {
                    if (prev + data[counter + 1, indexFirst + i] < energy)
                    {
                        energy = data[counter, indexFirst] + data[counter + 1, indexFirst + i];
                        res1[counter + 1] = indexFirst + i;
                    }

                }
                indexFirst = res1[counter + 1];
                counter++;
            }
            res1[data.GetLength(0) - 1] = res1[data.GetLength(0) - 2];

        }


    }


    public class Q3SeamCarving3 : Processor // Remove Seam
    {
        public Q3SeamCarving3(string testDataName) : base(testDataName) { }
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        public override string Process(string inStr)
        {
            // Parse input file
            string[] everyLine = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            int n = int.Parse(everyLine[0]);

            string[] temp = everyLine[1].Split(',');
            int x = temp.Length;
            decimal[,] data = new decimal[n, x];
            for (int i = 0; i < n; i++)
            {
                var tmp = everyLine[1 + i].Split(',');
                for (int j = 0; j < x; j++)
                {
                    data[i, j] =Math.Round(decimal.Parse(tmp[j])*1.0M,2);
                }
            }
            int number = int.Parse(everyLine[n + 1]);
            string toRemove = everyLine[n + 2];
            string[] vOrH = toRemove.Split(':');
            string[] strInx = vOrH[1].Split(',');
            int[] indexes = new int[strInx.Length];
            for (int i = 0; i < strInx.Length; i++)
            {
                indexes[i] = int.Parse(strInx[i]);
            }
            var solved = Solve(vOrH[0], indexes, data);

            string res = "";
            var r = new StringBuilder();
            for (int i = 0; i < solved.GetLength(0); i++)
            {
                res = "";
                for (int j = 0; j < solved.GetLength(1); j++)
                {
                    if (j != solved.GetLength(1) - 1)
                    {
                       
                            res += solved[i, j] + ",";
                    }

                    else
                    {
                       
                            res += solved[i, j];
                    }
                }
            
                r.AppendLine(res);
            }
            return r.ToString();
        }


        public decimal[,] Solve(string vOrH, int[] indexes, decimal[,] data)
        {
            if (vOrH == "v")
            {
                decimal[,] res1 = new decimal[data.GetLength(0), data.GetLength(1) - 1];
                removeVerticalSeam(data, indexes, res1);
                return res1;
            }

            else //=="h"
            {
                decimal[,] res2 = new decimal[data.GetLength(0) - 1, data.GetLength(1)];
                removeHorizontalSeam(data, indexes, res2);
                return res2;
            }

        }

        private void removeHorizontalSeam(decimal[,] data, int[] indexes, decimal[,] res2)
        {
            int m = 0, n = 0;
            for (int j = 0; j < data.GetLength(1); j++)
            {
                n = 0;
                for (int i = 0; i < data.GetLength(0); i++)
                {

                    if (i != indexes[j])
                        res2[n, m] = data[i, j];
                    else
                    {
                        res2[n, m] = data[i + 1, j];
                        i++;
                    }

                    n++;
                }
                m++;

            }
        }

        private void removeVerticalSeam(decimal[,] data, int[] indexes, decimal[,] res1)
        {
            int m = 0, n = 0;
            for (int j = 0; j < data.GetLength(0); j++)
            {
                n = 0;
                for (int i = 0; i < data.GetLength(1); i++)
                {

                    if (i != indexes[j])
                        res1[m, n] = data[j, i];
                    else
                    {
                        res1[m, n] = data[j, i + 1];
                        i++;
                    }

                    n++;
                }
                m++;

            }
        }
    }
}
