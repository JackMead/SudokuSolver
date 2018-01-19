using System;
using System.Collections.Generic;
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
            var sudoku = LoadSudoku();
            Console.WriteLine("Starting Sudoku:");
            PrintSudoku(sudoku);

            while (true)
            {
                UpdateAvailableOptions(sudoku);
            }

        }

        public void UpdateAvailableOptions(Sudoku sudoku)
        {
            RemoveRowOptions();
            RemoveColOptions();
            RemoveBoxOptions();
        }

        public void PrintSudoku(Sudoku sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (sudoku.Squares[row, col].Count == 1)
                    {
                        Console.Write(" {0} ", sudoku.Squares[row, col][0]);
                    }
                    else
                    {
                        Console.Write(" 0 ");
                    }
                }
                Console.WriteLine();
            }
        }

        public Sudoku LoadSudoku()
        {
            var path = "sudokuStartingVals.csv";
            var squares = new List<int>[9,9];
            using (var sr = new StreamReader(path))
            {
                var line = "";
                var lineNum = 0;
                while ((line = sr.ReadLine()) != null)
                {
                     var ints = line.Split(',');
                    for (int i = 0; i < 9; i++)
                    {
                        squares[lineNum,i]=new List<int>{int.Parse(ints[i])};
                    }
                    lineNum++;
                }
            }
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (squares[row, col].Contains(0))
                    {
                        squares[row, col] = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
                    }
                }
            }
            return new Sudoku(squares);
        }
    }

    public class Sudoku
    {
        public List<int>[,] Squares { get; set; }

        public Sudoku(List<int>[,] squares)
        {
            Squares = squares;
        }
    }
}
