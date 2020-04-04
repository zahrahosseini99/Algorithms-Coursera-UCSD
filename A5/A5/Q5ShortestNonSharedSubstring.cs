using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q5ShortestNonSharedSubstring : Processor
    {
        public Q5ShortestNonSharedSubstring(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, String>)Solve);

        private string Solve(string text1, string text2)
        {
            SuffixTrie trie = new SuffixTrie();
            trie.insert(text2);
            return trie.search(text1);
        }
    }

    internal class SuffixTrie
    {
        public Node root;
        public SuffixTrie()
        {
            root = new Node();
        }
        int inx;

        public void insert(string text)
        {
            var preV = root;
            for (int i = 0; i < text.Length; i++)
            {
                preV = root;
                for (int j = i; j < text.Length; j++)
                {
                    inx = (text[j] - 'A') % 5;
                    if (inx == 4)
                        inx = 3;
                    if (preV.child[inx] == null)
                    {
                        preV.child[inx] = new Node();
                        preV.child[inx].PreVisited = preV;
                    }
                    preV = preV.child[inx];
                }
              
            }
        }

        public string search(string text)
        {
            var preV = root;
            int subStringLength = int.MaxValue;
            int edgeTraveller = 0;
            int startindex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                preV = root;
                edgeTraveller = 0;
                for (int j = i; j < text.Length; j++)
                {
                    inx = (text[j] - 'A') % 5;
                    if (inx == 4)
                        inx = 3;
                    if (preV.child[inx] == null)
                    {
                        edgeTraveller++;
                        if (edgeTraveller < subStringLength)
                        {
                            startindex = j + 1 - edgeTraveller;
                            subStringLength = edgeTraveller;
                            break;
                        }
 }
                    else
                    {
                        edgeTraveller++;
                        preV = preV.child[inx];
                    }
                }
            }
            return text.Substring(startindex,subStringLength);
        }
    }

    public class Node
    {
        public int index;
        const int size = 4;
        public Node PreVisited;
        public Node[] child = new Node[size];
 
        public Node()
        {
            for (int i = 0; i < size; i++)
            {
                child[i] = null;
            }
          
        }
    }
}
