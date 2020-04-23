using System;
using System.Drawing;
using System.Collections.Generic;

namespace Exam1
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }

        public string[] Solve(string[] data)
        {
            int dimReduction = int.Parse(data[0].Split()[0]);
            char direction = char.Parse(data[0].Split()[1]);
            string imagePath = data[1];
            var img = Utilities.LoadImage(imagePath);
            var bmp = Utilities.ConvertImageToColorArray(img);
            var res = Solve(bmp, dimReduction, direction);
            Utilities.SavePhoto(res, imagePath, "../../../../asd", direction);
            return Utilities.ConvertColorArrayToRGBMatrix(res);
        }

        private static List<List<Color>> BuildList(Color[,] input, char dim)
        {
            throw new NotImplementedException();
        }

        public static Color[,] Solve(Color[,] input, int reduction, char direction)
        {
            throw new NotImplementedException();
        }



        // sequence of indices for horizontal seam
        public static int[] findHorizontalSeam(List<List<double>> energy)
        {
            throw new NotImplementedException();
        }


        // sequence of indices for vertical seam
        public static int[] findVerticalSeam(List<List<double>> energy)
        {
            throw new NotImplementedException();
        }

        // energy of pixel at column x and row y
        public static List<List<double>> ComputeEnergy(List<List<Color>> bmp)
        {
            throw new NotImplementedException();
        }

        public static int ArgMin(int t, int minIdx, List<List<double>> energy)
        {
            throw new NotImplementedException();
        }

        // remove horizontal seam from current picture
        public static void removeHorizontalSeam(int[] seam, ref List<List<Color>> bmp, ref List<List<double>> energy)
        {
            throw new NotImplementedException();
        }

        // remove vertical seam from current picture
        public static void removeVerticalSeam(int[] seam, ref List<List<Color>> bmp, ref List<List<double>> energy)
        {
            throw new NotImplementedException();
        }

    }
}
