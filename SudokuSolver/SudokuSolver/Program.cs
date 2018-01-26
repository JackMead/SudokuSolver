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
            var sudoku = LoadSudoku();
            Console.WriteLine("Starting Sudoku:");
            PrintSudoku(sudoku);

            var nums = FindAvailableNumbers(sudoku, 0, 3);
            foreach (var num in nums)
            {
                Console.Write(num);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Completing:");
            CompleteSudoku(sudoku);
            stopwatch.Stop();
            PrintSudoku(sudoku);
            Console.WriteLine("Program took, {0}",stopwatch.Elapsed);
        }

        public void CompleteSudoku(Sudoku sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                if (RecursivelyFillInNextNumberIfPossible(sudoku, 0, 0, i))
                {
                    break;
                }
            }
        }

        public bool RecursivelyFillInNextNumberIfPossible(Sudoku sudoku, int row, int col, int attempts)
        {
            //PrintSudoku(sudoku);

            var startingValue = sudoku.Squares[row, col];
            if (startingValue == 0)
            {
                //Find available squares for this square
                var availableSquares = FindAvailableNumbers(sudoku, row, col);
                if (availableSquares.Count <= attempts)
                {
                    return false;
                }
                sudoku.Squares[row, col] = availableSquares[attempts];
            }
            
            var newCol = col < 8 ? col + 1 : 0;
            var newRow = col < 8 ? row : row + 1;
            if (newRow == 9)
            {
                return true;
            }
            var optionsNextNumber = FindAvailableNumbers(sudoku, newRow, newCol);

            for (int i = 0; i < optionsNextNumber.Count; i++)
            {
                if (RecursivelyFillInNextNumberIfPossible(sudoku, newRow, newCol, i))
                {
                    return true;
                }
            }
            sudoku.Squares[row, col] = startingValue;
            return false;
        }

        public List<int> FindAvailableNumbers(Sudoku sudoku, int thisRow, int thisCol)
        {
            if (sudoku.Squares[thisRow, thisCol]!=0)
            {
                return new List<int>{sudoku.Squares[thisRow, thisCol]};
            }
            var availableNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            //Remove row duplicates
            for (int col = 0; col < 9; col++)
            {
                if (sudoku.Squares[thisRow, col] != 0)
                {
                    availableNumbers.Remove(sudoku.Squares[thisRow, col]);
                }
            }
            //Remove col duplicates
            for (int row = 0; row < 9; row++)
            {
                if (sudoku.Squares[row, thisCol] != 0)
                {
                    availableNumbers.Remove(sudoku.Squares[row, thisCol]);
                }
            }
            //Remove box duplicates
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var rowToCheck = 3*(thisRow / 3) + row;
                    var colToCheck = 3*(thisCol / 3) + col;
                    if (sudoku.Squares[rowToCheck, colToCheck] != 0)
                    {
                        availableNumbers.Remove(sudoku.Squares[rowToCheck, colToCheck]);
                    }
                }
            }
            return availableNumbers;
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
