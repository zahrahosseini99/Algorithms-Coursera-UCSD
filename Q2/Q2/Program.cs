using System;
using System.Collections.Generic;

namespace Q2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        public static void Table(string pattern)
        {
            Dictionary<char, int> res = new Dictionary<char, int>();
            for (int i = 0; i < pattern.Length; i++)
            {
                res.Add(pattern[i], pattern.Length - i - 1);
            }

        }
    }
}
