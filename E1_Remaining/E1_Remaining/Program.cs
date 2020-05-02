using System;
using System.Drawing;
using System.Collections.Generic;

namespace Exam1
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Solve(args);
        }

        public static void Solve(string[] data)
        {
            int dimReduction = int.Parse(data[1].Split()[0]);
            char direction = char.Parse(data[1].Split()[1]);
            string imagePath = data[0];
            string name = data[2];
            var img = Utilities.LoadImage(imagePath);
            var bmp = Utilities.ConvertImageToColorArray(img);
            var Lcolor = BuildList(bmp, direction);
            var res = Solve(Lcolor, dimReduction, direction);
            Utilities.SavePhoto(res, imagePath, name, direction);
           
        }

        private static List<List<Color>> BuildList(Color[,] input, char dim)
        {
            List<List<Color>> res1 = new List<List<Color>>();
            List<List<Color>> res2 = new List<List<Color>>();
            if (dim == 'H')
            {
                for (int i = 0; i < input.GetLength(0); i++)
                {
                    res1.Add(new List<Color>());
                    for (int j = 0; j < input.GetLength(1); j++)
                    {
                        res1[i].Add(input[i, j]);
                    }
                }
                return res1;
            }
            else//=='v'
            {
                for (int i = 0; i < input.GetLength(0); i++)
                {
                    res2.Add(new List<Color>());
                    for (int j = 0; j < input.GetLength(1); j++)
                    {
                        res2[i].Add(input[i, j]);
                    }
                }
                return res2;
            }
        }

        public static Color[,] Solve(List<List<Color>> input, int reduction, char direction)
        {
            var eng = ComputeEnergy(input, direction);


            if (direction == 'V')
            {
                for (int i = 0; i < reduction; i++)
                {
                    var seam = findVerticalSeam(eng);

                    removeVerticalSeam(seam, eng, input);

                }

            }
            else//==h
            {
                for (int i = 0; i < reduction; i++)
                {
                    var seam = findHorizontalSeam(eng);

                    removeHorizontalSeam(seam, eng, input);


                }
            }
            var result = inputToArray(input, direction);
            return result;
        }

        private static Color[,] inputToArray(List<List<Color>> input, char direction)
        {
            Color[,] res;
            if (direction == 'V')
            {
                res = new Color[input[0].Count, input.Count];
                for (int i = 0; i < input[0].Count; i++)
                {
                    for (int j = 0; j < input.Count; j++)
                    {
                        res[i, j] = input[j][i];
                    }

                }
            }
            else
            {
                res = new Color[input.Count, input[0].Count];
                for (int i = 0; i < input.Count; i++)//x
                {
                    for (int j = 0; j < input[0].Count; j++)//y
                    {
                        res[i, j] = input[i][j];
                    }
                }
            }
            return res;

        }



        // sequence of indices for horizontal seam
        public static int[] findHorizontalSeam(List<List<double>> data)
        {
            int[] res2 = new int[data.Count];

            int indexFirst = 0;
            double eng = 1000;
            for (int i = 0; i < data[0].Count; i++)
            {
                if (eng > data[1][i])
                {
                    eng = data[1][i];
                    indexFirst = i;
                }
            }
            res2[0] = indexFirst;
            res2[1] = indexFirst;
            int counter = 1;

            double prev = 0;

            while (counter < data.Count - 1)
            {
                prev = data[counter][indexFirst];
                double energy = double.PositiveInfinity; ;
                for (int i = -1; i < 2; i++)
                {
                    if (prev + data[counter + 1][indexFirst + i] < energy)
                    {
                        energy = data[counter][indexFirst] + data[counter + 1][indexFirst + i];
                        res2[counter + 1] = indexFirst + i;
                    }

                }
                indexFirst = res2[counter + 1];
                counter++;
            }
            res2[data.Count - 1] = res2[data.Count - 2];
            return res2;
        }


        // sequence of indices for vertical seam
        public static int[] findVerticalSeam(List<List<double>> data)
        {
            int[] res1 = new int[data.Count];
            int indexFirst = 0;
            double eng = 1000;
            for (int i = 0; i < data[0].Count; i++)
            {
                if (eng > data[1][i])
                {
                    eng = data[1][i];
                    indexFirst = i;
                }
            }
            res1[0] = indexFirst;
            res1[1] = indexFirst;
            int counter = 1;

            double prev = 0;

            while (counter < data.Count - 1)
            {
                prev = data[counter][indexFirst];
                double energy = double.PositiveInfinity; ;
                for (int i = -1; i < 2; i++)
                {
                    if (prev + data[counter + 1][indexFirst + i] < energy)
                    {
                        energy = data[counter][indexFirst] + data[counter + 1][indexFirst + i];
                        res1[counter + 1] = indexFirst + i;
                    }

                }
                indexFirst = res1[counter + 1];
                counter++;
            }
            res1[data.Count - 1] = res1[data.Count - 2];
            return res1;
        }

        // energy of pixel at column x and row y
        public static List<List<double>> ComputeEnergy(List<List<Color>> data, char dir)
        {
            List<List<double>> res = new List<List<double>>();
            if (dir == 'V')
            {
                for (int i = 0; i < data.Count; i++)
                {
                    res.Add(new List<double>());
                    for (int j = 0; j < data[0].Count; j++)
                    {
                        if ((i == 0) || (j == 0) || (i == data.Count - 1) || (j == data[0].Count - 1))
                            res[i].Add(1000);
                        else
                        {
                            double rX = (double)data[i][j + 1].A - (double)data[i][j - 1].A;
                            double rY = (double)data[i + 1][j].A - (double)data[i - 1][j].A;

                            double gX = (double)data[i][j + 1].A - (double)data[i][j - 1].A;
                            double gY = (double)data[i + 1][j].A - (double)data[i - 1][j].A;

                            double bX = (double)data[i][j + 1].A - (double)data[i][j - 1].A;
                            double bY = (double)data[i + 1][j].A - (double)data[i - 1][j].A;

                            double difX = (double)Math.Pow((double)rX, 2) + (double)Math.Pow((double)gX, 2) + (double)Math.Pow((double)bX, 2);
                            double difY = (double)Math.Pow((double)rY, 2) + (double)Math.Pow((double)gY, 2) + (double)Math.Pow((double)bY, 2);

                            double energy = Math.Sqrt((double)difX + (double)difY);
                            res[i].Add(Math.Round(energy, 3));
                        }

                    }
                }
            }
            else//h
            {


                for (int i = 0; i < data.Count; i++)
                {
                    res.Add(new List<double>());
                    for (int j = 0; j < data[0].Count; j++)
                    {
                        if ((i == 0) || (j == 0) || (i == data.Count - 1) || (j == data[0].Count - 1))
                            res[i].Add(1000);
                        else
                        {
                            double rY = (double)data[i][j + 1].A - (double)data[i][j - 1].A;
                            double rX = (double)data[i + 1][j].A - (double)data[i - 1][j].A;

                            double gY = (double)data[i][j + 1].A - (double)data[i][j - 1].A;
                            double gX = (double)data[i + 1][j].A - (double)data[i - 1][j].A;

                            double bY = (double)data[i][j + 1].A - (double)data[i][j - 1].A;
                            double bX = (double)data[i + 1][j].A - (double)data[i - 1][j].A;

                            double difX = (double)Math.Pow((double)rX, 2) + (double)Math.Pow((double)gX, 2) + (double)Math.Pow((double)bX, 2);
                            double difY = (double)Math.Pow((double)rY, 2) + (double)Math.Pow((double)gY, 2) + (double)Math.Pow((double)bY, 2);

                            double energy = Math.Sqrt((double)difX + (double)difY);
                            res[i].Add(Math.Round(energy, 3));
                        }

                    }
                }

            }
            return res;
        }

        public static int ArgMin(int t, int minIdx, List<List<double>> energy)
        {
            throw new NotImplementedException();
        }

        // remove horizontal seam from current picture
        public static void removeHorizontalSeam(int[] seam, List<List<double>> eng, List<List<Color>> img)
        {
            for (int i = 0; i < img.Count; i++)
            {
                img[i].RemoveAt(seam[i]);
                eng[i].RemoveAt(seam[i]);
            }


        }

        // remove vertical seam from current picture
        public static void removeVerticalSeam(int[] seam, List<List<double>> eng, List<List<Color>> img)
        {

            for (int i = 0; i < img.Count; i++)
            {
                img[i].RemoveAt(seam[i]);
                eng[i].RemoveAt(seam[i]);
            }


        }
    }

}
