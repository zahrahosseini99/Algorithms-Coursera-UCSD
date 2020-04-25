using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q3PatternMatchingSuffixArray : Processor
    {
        public Q3PatternMatchingSuffixArray(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, string[], long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, long n, string[] patterns)
        {
            text = text + "$";
            long[] suffixArray = BuildSuffixArray(text);
            List<long> result = new List<long>();
            for (int i = 0; i < patterns.Length; i++)
            {
                BinarySearch(text, patterns[i], suffixArray, result, 0, suffixArray.Length - 1);
            }

            if (result.Count == 0)
                result.Add(-1);
            return result.Distinct().Select(x => x).ToArray();
        }

        private void BinarySearch(string text, string pattern, long[] suffixArray, List<long> result, int low, int high)
        {
            if (high >= low)
            {
                int mid = low + (high - low) / 2;
                int cmp = string.Compare(pattern, 0, text, (int)suffixArray[mid], pattern.Length);
                if (cmp == 0)
                {
                    result.Add(suffixArray[mid]);
                    BinarySearch(text, pattern, suffixArray, result, low, mid - 1);
                    BinarySearch(text, pattern, suffixArray, result, mid + 1, high);
                }
                else if (cmp > 0)
                {
                    BinarySearch(text, pattern, suffixArray, result, mid + 1, high);
                }
                else
                {
                    BinarySearch(text, pattern, suffixArray, result, low, mid - 1);
                }
            }
            return;
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
            else if (c == 'A')
                res = 1;
            else if (c == 'C')
                res = 2;
            else if (c == 'G')
                res = 3;
            else
                res = 4;
            return res;

        }
    }
}
