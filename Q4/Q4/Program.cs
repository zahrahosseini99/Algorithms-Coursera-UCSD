using System;
using System.Collections.Generic;

namespace Q4
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph();
            Random rand = new Random();
            Func<int, int, bool>[] Algorithms = new Func<int, int, bool>[3];

            Algorithms[0] = (x, y) => true;
            Algorithms[1] = (x, y) => ((x % 2 == 0) && (y % 2 == 0)) || ((x % 2 != 0) && (y % 2 != 0)) ? true : false;
            Algorithms[2] = (x, y) => ((x - y) % 2 == 1) ? true : false;
            
            for (int i = 0; i < 3; i++)
            {

                int nodeNumsToInsert = rand.Next(3, 8);
                int algorithmNumber = rand.Next(3);
                g.InsertToGraph(Algorithms[algorithmNumber], nodeNumsToInsert);


            }

        }
    }

    public class Graph
    {
        public List<Node> nodes;
        public  int  nodesCount;
        public delegate void Insert(int numberToinsert);
        public Graph()
        {
            this.nodes = new List<Node>();
            this.nodesCount = 0;
        }

        internal void InsertToGraph(Func<int, int, bool> algorithmChooser, int nodeNumsToInsert)
        {
            int counter = nodesCount;
            while (counter < counter + nodeNumsToInsert)
            {
                nodes.Add(new Node(counter++));
                for (int i = nodesCount + 1; i < counter; i++)
                {
                    if (algorithmChooser(counter, i))
                    {
                        this.nodes[counter].adj.Add(new Node(i));
                        this.nodes[i].adj.Add(new Node(counter));
                    }
                }

            }

        }
    }

    public class Node
    {
        public List<Node> adj;
        public int nodeNumber;
        public Node(int n)
        {
            nodeNumber = n;
        }
    }
}
