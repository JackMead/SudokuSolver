using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        public void Run()
        {
            Console.WriteLine("Loading Sudoku");
            var sudoku = LoadSudoku();
            Console.WriteLine("Loaded Sudoku:");
            PrintSudoku(sudoku);

            var solver = new Solver();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Working on sudoku:");
            solver.Complete(sudoku);
            stopwatch.Stop();
            PrintSudoku(sudoku);
            Console.WriteLine("Program took, {0}ms",stopwatch.ElapsedMilliseconds);
        }

        public void PrintSudoku(Sudoku sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Console.Write(" {0} ", sudoku.Squares[row, col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public Sudoku LoadSudoku()
        {
            var path = "sudokuStartingVals.csv";
            var squares = new int[9, 9];
            using (var sr = new StreamReader(path))
            {
                var line = "";
                var lineNum = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var ints = line.Split(',');
                    for (int i = 0; i < 9; i++)
                    {
                        squares[lineNum, i] = int.Parse(ints[i]);
                    }
                    lineNum++;
                }
            }
            return new Sudoku(squares);
        }
    }

    public class Sudoku
    {
        public int[,] Squares { get; set; }

        public Sudoku(int[,] squares)
        {
            Squares = squares;
        }
    }
}
