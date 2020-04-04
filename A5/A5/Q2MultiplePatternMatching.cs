using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q2MultiplePatternMatching : Processor
    {
        public Q2MultiplePatternMatching(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(10, 50);
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, string[] patterns)
        {
            Trie tree = new Trie(n);
            List<long> res = new List<long>();
            Node preVistit = tree.Root;
            int inx;
            for (int i = 0; i < patterns.Length; i++)
            {
                tree.insert(patterns[i]);
            }

            for (int i = 0; i < text.Length; i++)
            {
                preVistit = tree.Root;
                for (int j = i; j < text.Length; j++)
                {
                    inx = (text[j] - 'A') % 5;
                    if (inx == 4)
                        inx = 3;

                    if (preVistit.child[inx]== null)
                        break;

                    if (tree.search(text[j], preVistit))
                    {

                        res.Add(i);
                    }
                   
                    preVistit = preVistit.child[inx];

                }

            }

            if (res.Count == 0)
            {
                res.Add(-1);
                return res.ToArray();
            }

            return res.Distinct().Select(x => x).ToArray();
        }
        internal class Trie
        {
            private long NodeCount;
            public Node Root;
            public Trie(long n)
            {
                this.NodeCount = n;
                Root = new Node();
                Root.PreVisited = null;
            }

            internal void insert(string str)
            {
                int inx;
                Node preV = Root;

                for (int i = 0; i < str.Length; i++)
                {
                    inx = (str[i] - 'A') % 5;
                    if (inx == 4)
                        inx = 3;
                    if (preV.child[inx] == null)
                    {

                        preV.child[inx] = new Node();
                        preV.child[inx].PreVisited = preV;
                    }


                    preV = preV.child[inx];
                }
                preV.IsLastChar = true;
            }

            internal bool search(char c, Node preVistit)
            {
                int inx;

                inx = (c - 'A') % 5;
                if (inx == 4)
                    inx = 3;
                if (preVistit.child[inx] == null)
                    return false;
                else if (preVistit.child[inx].IsLastChar)
                    return true;

                return false;


            }
        }

        internal class Node
        {

            public int index;
            const int size = 4;
            public Node PreVisited;
            public Node[] child = new Node[size];
            public bool IsLastChar;
            public Node()
            {
                for (int i = 0; i < size; i++)
                {
                    child[i] = null;
                }
                IsLastChar = false;
            }

        }

    }


}
