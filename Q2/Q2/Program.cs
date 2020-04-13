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
            List<int> res = new List<int>();
            Dictionary<char, int> table = new Dictionary<char, int>();
            for (int i = 0; i < pattern.Length; i++)
            {
                table.Add(pattern[i], pattern.Length - i - 1);
            }

        }
        public void boyerMoore(string text, Dictionary<char, int> table)
        {

        }
    }
}
