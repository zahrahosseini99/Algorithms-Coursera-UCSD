using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q4SuffixTree : Processor
    {
        public Q4SuffixTree(string testDataName) : base(testDataName)
        {

            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String[]>)Solve);

        public string[] Solve(string text)
        {
            Suffix tree = new Suffix();
            tree.insert(text);
            return tree.dfs();
        }
    }

    internal class Suffix
    {
        public List<Edge>[] SuffixTree = new List<Edge>[50000];
        int indexCounter;
        public Suffix()
        {
            for (int i = 0; i < 48000; i++)
            {
                SuffixTree[i] = new List<Edge>();
            }
        }
        public void insert(string text)
        {
            indexCounter = 0;
            int preVisited = 0;
            int equalityIndex = 0;
            int edgeTraveller = 0;
            int subStringTraveller = 0;
            int edgelength = 0;
            bool isPresent = false;
            bool extendedEdge = false;
            bool checking = false;
            SuffixTree[indexCounter] = new List<Edge>();
            indexCounter++;
            for (int textTraveller = 0; textTraveller < text.Length; textTraveller++)
            {
                subStringTraveller = textTraveller;
                preVisited = 0;
                while (subStringTraveller < text.Length)
                //for (subStringTraveller = textTraveller; subStringTraveller < text.Length; subStringTraveller++)
                {
                    isPresent = false;
                    for (int childChecker = 0; childChecker < SuffixTree[preVisited].Count; childChecker++)
                    {
                        if (text[subStringTraveller] == text[SuffixTree[preVisited][childChecker].Start])
                        {
                            isPresent = true;
                            equalityIndex = childChecker;
                            break;
                        }

                    }
                    if (!isPresent)
                    {

                        SuffixTree[preVisited].Add(new Edge(indexCounter, subStringTraveller, text.Length - subStringTraveller,
                            text.Substring(subStringTraveller, text.Length - subStringTraveller)));
                        indexCounter++;
                        break;
                    }
                    else
                    {
                        edgeTraveller = SuffixTree[preVisited][equalityIndex].Start;
                        edgelength = SuffixTree[preVisited][equalityIndex].Start + SuffixTree[preVisited][equalityIndex].Length;
                        while ((subStringTraveller < text.Length) && (edgeTraveller < edgelength))
                        {
                            if (text[subStringTraveller] == text[edgeTraveller])
                            {
                                subStringTraveller++;
                                edgeTraveller++;
                                checking = true;
                                if (edgeTraveller >= edgelength)
                                {
                                    preVisited = SuffixTree[preVisited][equalityIndex].Index;
                                    break;
                                }

                            }
                            else
                            {
                                int start = SuffixTree[preVisited][equalityIndex].Start;
                                int length = SuffixTree[preVisited][equalityIndex].Length;
                                int index = SuffixTree[preVisited][equalityIndex].Index;

                                //[I]


                                SuffixTree[preVisited][equalityIndex].Length = edgeTraveller - start;
                                SuffixTree[preVisited][equalityIndex].Index = indexCounter;
                                SuffixTree[preVisited][equalityIndex].Text = text.Substring(start, edgeTraveller - start);

                                //[II]

                                indexCounter++;
                                SuffixTree[indexCounter - 1].Add(new Edge(indexCounter, subStringTraveller, text.Length - subStringTraveller,
                                    text.Substring(subStringTraveller, text.Length - subStringTraveller)));
                                //[III]


                                SuffixTree[indexCounter - 1].Add(new Edge(index, edgeTraveller, (start + length) - edgeTraveller,
                                    text.Substring(edgeTraveller, length - (edgeTraveller - start))));
                                indexCounter++;

                                extendedEdge = true;
                                break;
                            }
                        }
                        if (extendedEdge)
                        {
                            extendedEdge = false;
                            break;
                        }
                    }
                    //if (checking)
                    //{
                    //    checking = false;
                    //    break;
                    //}
                }
            }

        }

        internal string[] dfs()
        {
            Stack<Edge> DFS = new Stack<Edge>();

            for (int i = 0; i < SuffixTree[0].Count; i++)
            {
                DFS.Push(SuffixTree[0][i]);
            }
            Edge temp;
            int counter = 0;
            string[] result = new string[indexCounter];
            while (DFS.Count != 0)
            {
                temp = DFS.Pop();
                result[counter] = temp.Text;
                foreach (var item in SuffixTree[temp.Index])
                {
                    DFS.Push(item);
                }
                counter++;
            }
            return result;
        }
    }

    internal class Edge
    {
        public int Index;
        public int Start;
        public int Length;
        public string Text;
        public Edge(int _index, int _start, int _length, string _txt)
        {
            Start = _start;
            Index = _index;
            Length = _length;
            Text = _txt;
        }

    }
}
