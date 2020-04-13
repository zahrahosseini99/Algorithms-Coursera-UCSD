using System;
using System.Collections.Generic;

namespace Q2
{
    class Program
    {
        static void Main(string[] args)
        {
            string txt = "WELCOMETOTEAMMAST";
            string pat = "TEAMMAST";
            Dictionary<char, int> table = new Dictionary<char, int>();
            table = Table(pat);
            Console.WriteLine(boyerMoore(txt, pat, table));
        }
        public static Dictionary<char, int> Table(string pattern)
        {

            Dictionary<char, int> table = new Dictionary<char, int>();
            for (int i = 0; i < pattern.Length; i++)
            {
                table[pattern[i]] = Math.Max(1, pattern.Length - i - 1);
            }
            table['*'] = pattern.Length;
            return table;
        }
        public static int boyerMoore(string text, string pattern, Dictionary<char, int> table)
        {
            int shift = 0;
            int res = 0;
            while (shift <= (text.Length - pattern.Length))
            {
                int j = pattern.Length - 1;
                while (j >= 0 && pattern[j] == text[shift + j])
                {
                    j--;
                }
                if (j < 0)
                {
                   res=shift;
                    break;
                }
                else
                {
                    shift += Math.Max(1, j - table[text[shift + j - 1]]);
                }
            }
            return res;

        }
    }
}
