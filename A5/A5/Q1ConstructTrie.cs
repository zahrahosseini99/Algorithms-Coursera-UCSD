using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q1ConstructTrie : Processor
    {
        public Q1ConstructTrie(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, String[], String[]>)Solve);

        public string[] Solve(long n, string[] patterns)
        {
            List<string> res = new List<string>();
            Trie tree = new Trie(n);
            int count = 1;
            for (int i = 0; i < patterns.Length; i++)
            {
                tree.insert(patterns[i],ref count, res);
            }
            return res.ToArray();
        }
        internal class Trie
        {
            private long NodeCount;
            private Node Root;
            public Trie(long n)
            {
                this.NodeCount = n;
                Root = new Node(0);
                Root.PreVisited = null;
            }

            internal void insert(string str, ref int count, List<string> res)
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

                        preV.child[inx] = new Node(count);
                        preV.child[inx].PreVisited = preV;
                        res.Add(preV.child[inx].PreVisited.index.ToString() + "->" + preV.child[inx].index.ToString() + ":" + str[i]);
                        count++;

                    }


                    preV = preV.child[inx];
                }
                preV.IsLastChar = true;
            }
        }

        internal class Node
        {

            public int index;
            const int size = 4;
            public Node PreVisited;
            public Node[] child = new Node[size];
            public bool IsLastChar;
            public Node(int inx)
            {
                for (int i = 0; i < size; i++)
                {
                    child[i] = null;
                }
                IsLastChar = false;
                index = inx;

            }

        }

   
    }
}
