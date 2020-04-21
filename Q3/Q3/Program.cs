using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ta_class
{
    public class Program
    {
        public static void Main(string[] args)
        {

        }
        public static int[] search(string text,
                          string pattern)
        {
            int[] resualts = { };
            List<int> res = new List<int>();
            string concat = pattern + "$" + text;
            int[] z = new int[concat.Length];
            z[0] = -1;
            for (int i = 1; i < concat.Length; i++)
            {
                int count = 0;
                int j = i;
                while (concat[count] == concat[j])
                {
                    count++;
                    j++;
                }
                z[i] = count;
                if (z[i] == pattern.Length)
                    res.Add(i - (pattern.Length + 1));

            }

            return res.ToArray();
        }
    }
}
