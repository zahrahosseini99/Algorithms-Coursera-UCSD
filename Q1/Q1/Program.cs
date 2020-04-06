using System;

namespace Q1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string AB = Console.ReadLine();
            //string[] ABS = AB.Split();
            //string A = ABS[0];
            //string B = ABS[1];
            //Console.WriteLine(hamming(A, B));
        }
        public static string hamming(string A, string B)
        {
            string result = "";
            int counter = 0;
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] != B[i])
                    counter++;

            }
            if (counter % 2 != 0)
                result = "NOT POSSIBLE";


            else
            {
                int chose = counter / 2;

                for (int i = 0; i < A.Length; i++)
                {
                    if (A[i] != B[i] && chose > 0)
                    {
                        result += B[i];
                        chose--;
                    }
                    else
                    {
                        result += A[i];
                    }

                }

            }
            return result;

        }
    }
}
