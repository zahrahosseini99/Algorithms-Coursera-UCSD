using System;
using System.Collections.Generic;
using TestCommon;

namespace E2
{
    public class Q2BoardGame : Processor
    {
        public Q2BoardGame(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, long[,], string[]>)Solve);

        public string[] Solve(int BoardSize, long[,] Board)
        {
            List<string> clauses = new List<string>();
            //3 ta halat darim 1 2 3 vase har khone
            long[,,] table = new long[BoardSize, BoardSize, 3];
            fillingTbale(table, BoardSize, Board);
            checkingEachCell(table, Board, BoardSize, clauses);
            checkingEachRow(table, Board, BoardSize, clauses);
            checkingEachColumn(table, Board, BoardSize, clauses);
            checkingColor(table, Board, BoardSize, clauses);
          
            String[] res = new string[clauses.Count + 1];
            res[0] = $"{clauses.Count} {BoardSize * BoardSize * 3}";
            for (int i = 0; i < clauses.Count; i++)
            {
                res[i + 1] = clauses[i];
            }
            return res;
           
        }

        private void checkingColor(long[,,] table, long[,] board, int boardSize, List<string> clauses)
        {
            //int color = 0;
            for (int j = 0; j < boardSize; j++)
            {
                for (int i = 0; i < boardSize; i++)
                {
                    for (int k = i + 1; k < boardSize; k++)
                    {
                        clauses.Add($"-{table[i, j, 1]} -{table[k, j, 2]}" + " 0");
                        clauses.Add($"-{table[i, j, 2]} -{table[k, j, 1]}" + " 0");
                    }
                }

            }
        }

        private void checkingEachColumn(long[,,] table, long[,] board, int boardSize, List<string> clauses)
        {

            //long[] array = new long[boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                List<long> arr = new List<long>();
                for (int j = 0; j < boardSize;j++)
                {
                    if (board[j, i] != 1)
                        arr.Add(table[j, i, board[j, i] - 1]);
                }
                // if (arr.Count != 0)
                clauses.Add(string.Join(" ", arr) + " 0");


            }
        }

        private void checkingEachRow(long[,,] table, long[,] board, int boardSize, List<string> clauses)
        {
            //long[] array = new long[boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                List<long> arr = new List<long>();
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] != 1)
                        arr.Add(table[i, j, board[i, j] - 1]);
                }
                // if(arr.Count!=0)
                clauses.Add(string.Join(" ", arr) + " 0");


            }
        }

        private void checkingEachCell(long[,,] table, long[,] board, int boardSize, List<string> clauses)
        {

            long[] array = new long[3];
            // int color = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        array[k] = table[i, j, k];
                    }
                    clauses.Add(string.Join(" ", array) + " 0");
                }
            }
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    clauses.Add($"-{table[i, j, 0]} -{table[i, j, 1]}" + " 0");
                    clauses.Add($"-{table[i, j, 0]} -{table[i, j, 2]}" + " 0");
                    clauses.Add($"-{table[i, j, 1]} -{table[i, j, 2]}" + " 0");
                }
            }
        }

        private void fillingTbale(long[,,] table, int boardSize, long[,] board)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        table[i, j, k] = GetVarNum(i, j, k, boardSize);
                    }

                }
            }
        }

        private long GetVarNum(int i, int j, int k, int boardSize)
        {
            return i * 3 * boardSize + j * 3 + k + 1;
        }



        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
    }
}
