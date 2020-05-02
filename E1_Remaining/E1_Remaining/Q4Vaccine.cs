using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q4Vaccine : Processor
    {
        public Q4Vaccine(string testDataName) : base(testDataName) { this.ExcludeTestCaseRangeInclusive(2, 106); }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<string, string, string>)Solve);

        public string Solve(string dna, string pattern)
        {
            ///////SUFFIX ARRAY

            //string text = dna + "$";
            //long[] suffixArray = new long[text.Length];

            //suffixArray = BuildSuffixArray(text);

            //////LCP
            //long[] LCP = ComputeLC(text,pattern, suffixArray);

            //////LCS
            //char[] charArray = text.ToCharArray();
            //Array.Reverse(charArray);
            //string reverseText= new string(charArray);

            //long[] LCS = ComputeLC(reverseText,pattern,suffixArray);


            //List<long> res = new List<long>();

            //string result = "";
            //for (int i = 0; i < suffixArray.Length; i++)
            //{
            //    string subString = dna.Substring((int)suffixArray[i]);

            //    if ((subString.Length == pattern.Length) &&
            //        (LCP[(int)suffixArray[i]] + LCS[(int)suffixArray[i]] >= pattern.Length - 1))
            //        res.Add(i);

            //}

            //if (res.Count == 0)
            //    return "No Match!";
            //else
            //{

            //    for (int i = 0; i < res.Count; i++)
            //    {
            //        result += res[i] + " ";
            //    }
            //}
            // return result.TrimEnd();
            return "No Match!";
        }

        private long[] ComputeLC(string text,string pattern, long[] suffixArray)
        {
            int n = text.Length;
            long lcp = 0;
            long[] lcpArray = new long[n - 1];
            long[] inverseSuffixArray = new long[n];
            inverseSuffixArray = InvertSuffixArray(suffixArray);
            long suffix = suffixArray[0];
            for (int i = 0; i < text.Length; i++)
            {
                long orderIndex = inverseSuffixArray[suffix];
                if(orderIndex==n-1)
                {
                    lcp = 0;
                    suffix = (suffix + 1) % n;
                    continue;
                }
                string nextsuffix = pattern;
                //pattern
                lcp = LCPofSuffixes(text,suffix,nextsuffix,lcp-1);
                lcpArray[orderIndex] = lcp;
                suffix = (suffix + 1) % n;
                   
            }
            return lcpArray;

        }

        private long LCPofSuffixes(string text, long i, string pattern, long v)
        {
            long lcp = Math.Max(0, v);
            int j = 0;
            while (i+lcp<text.Length && j+lcp<pattern.Length)
            {
                if(text[(int)(i +lcp)]==pattern[(int)(j+lcp)])
                {
                    lcp = lcp + 1;
                }
                else
                {
                    break;
                }
            }
            return lcp;
        }

        private long[] InvertSuffixArray(long[] suffixArray)
        {
            long[] pos = new long[suffixArray.Length];
            for (int i = 0; i < pos.Length; i++)
            {
                pos[suffixArray[i]] = i;
            }
            return pos;
        }

        private long[] BuildLC(string text, long[] suffixArray)
        {
            int n = suffixArray.Length;
            long[] lcp = new long[n-1];
            long[] inverseSuffixArray = new long[n];
            
            for (int i = 0; i < n; i++)
            {
                inverseSuffixArray[suffixArray[i]] = i;
              
            }
            int counter = 0;

            for (int i = 0; i < n; i++)
            {
                if(inverseSuffixArray[i]==n-1)
                {
                    counter = 0;
                    continue;
                }
                long j = suffixArray[inverseSuffixArray[i] + 1];

                while (i+counter<n && j+counter<n && text[i+counter]==text[(int)j+counter])
                {
                    counter = counter + 1;

                }
                lcp[inverseSuffixArray[i]] = counter;

                if (counter > 0)
                {
                    counter--;
                }
            }
            return lcp;
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
            else 
                res = 2;
           
            return res;

        }
    }
}
