using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q4Vaccine : Processor
    {
        public Q4Vaccine(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<string, string, string>)Solve);

        public string Solve(string dna, string pattern)
        {
           string bwt =  "";
            long[] sufArray = new long[dna.Length];
            string text = dna + "$";
            sufArray = BuildSuffixArray(text);
            for (int i = 0; i < dna.Length; i++)
            {
                bwt += dna[(int)(sufArray[i] - 1 + dna.Length) % dna.Length];
            }

            return "nothing";
            //pattern matching

          //  long[] res = new long[n];
            long[] ranks = new long[text.Length];
            // long[] ranks = new long[bwt.Length];
            Dictionary<char, long> counts = new Dictionary<char, long>();
            counts.Add('$', 0);
            counts.Add('A', 0);
            counts.Add('C', 0);
            counts.Add('G', 0);
            counts.Add('T', 0);

            for (int i = 0; i < text.Length; i++)
            {
                ranks[i] = counts[text[i]];
                counts[text[i]] += 1;

            }

            Dictionary<char, long> firstColumn = new Dictionary<char, long>();
            long ranker = 0;
            foreach (var item in counts)
            {
                firstColumn.Add(item.Key, ranker);
                ranker += item.Value;
            }

           
                char c = pattern[pattern.Length - 1];
                long top = firstColumn[c];
                long bottom = firstColumn[c] + counts[c] - 1;

           //     res= bwPatternMatching(text, pattern, top, bottom, firstColumn, ranks);
           
           // return res;

        }
        private long[] BuildSuffixArray(string text)
        {
            long[] order = new long[text.Length];
            order = CountingSort(text);

            long[] grade = new long[text.Length];
            grade = ComputeCharClasses(text, order);
            long l = 1;
            while (l < text.Length)
            {
                order = SortDoubled(text, l, order, grade);
                grade = UpdateClasses(order, grade, l);
                l = 2 * l;
            }
            return order;
        }

        private long[] UpdateClasses(long[] order, long[] grade, long l)
        {
            long n = order.Length;
            long[] newGrade = new long[n];
            newGrade[order[0]] = 0;
            for (int i = 1; i < n; i++)
            {
                long cur = order[i];
                long prev = order[i - 1];
                long mid = (cur + l);
                long midPreV = (prev + l) % n;
                if ((grade[cur] != grade[prev]) || (grade[mid] != grade[midPreV]))
                {
                    newGrade[cur] = newGrade[prev] + 1;
                }
                else
                {
                    newGrade[cur] = newGrade[prev];
                }

            }
            return newGrade;
        }

        private long[] SortDoubled(string text, long l, long[] order, long[] grade)
        {
            long[] count = new long[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                count[i] = 0;
            }
            long[] newOrder = new long[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                count[grade[i]] = count[grade[i]] + 1;
            }
            for (int j = 1; j < text.Length; j++)
            {
                count[j] = count[j] + count[j - 1];
            }
            long g = 0;
            long start = 0;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                start = ((order[i] - l + text.Length) % (text.Length));
                g = grade[(int)start];
                count[g] = count[g] - 1;
                newOrder[count[g]] = start;
            }
            return newOrder;
        }

        private long[] ComputeCharClasses(string text, long[] order)
        {
            long[] grade = new long[text.Length];
            grade[order[0]] = 0;
            for (int i = 1; i < text.Length; i++)
            {

                if (text[(int)order[i]] != text[(int)order[i - 1]])
                {
                    grade[order[i]] = grade[order[i - 1]] + 1;
                }
                else
                {
                    grade[order[i]] = grade[order[i - 1]];
                }
            }
            return grade;
        }

        private long[] CountingSort(string text)
        {
            long[] order = new long[text.Length];
            long[] count = new long[5];
            for (int i = 0; i < 5; i++)
            {
                count[i] = 0;
            }
            for (int i = 0; i < text.Length; i++)
            {
                int inx = findIndex(text[i]);
                count[inx] = count[inx] + 1;
            }
            for (int i = 1; i < 5; i++)
            {
                count[i] = count[i] + count[i - 1];
            }
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                int inx = findIndex(c);
                count[inx] = count[inx] - 1;
                order[count[inx]] = i;
            }
            return order;
        }

        private int findIndex(char c)
        {
            int res = 0;
            if (c == '$')
                res = 0;
            else if (c == 'a')
                res = 1;
            else if (c == 'b')
                res = 2;
            else if (c == 'G')
                res = 3;
            else
                res = 4;
            return res;

        }
    }
}
