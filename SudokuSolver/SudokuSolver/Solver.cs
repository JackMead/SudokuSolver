using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Solver
    {
        public void Complete(Sudoku sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                if (RecursivelyFillInNextNumberOptimised(sudoku, 0, 0, i))
                {
                    break;
                }
            }
        }

        public bool RecursivelyFillInNextNumberOptimised(Sudoku sudoku, int row, int col, int attempts)
        {
            var startingValue = sudoku.Squares[row, col];
            if (startingValue == 0)
            {
                var availableSquares = FindAvailableNumbers(sudoku, row, col);
                if (availableSquares.Count <= attempts)
                {
                    return false;
                }
                sudoku.Squares[row, col] = availableSquares[attempts];
            }

            if (IsFinished(sudoku))
            {
                return true;
            }
            var nextCoord = FindNextSquare(sudoku);

            var optionsNextNumber = FindAvailableNumbers(sudoku, nextCoord.Row, nextCoord.Col);

            for (int i = 0; i < optionsNextNumber.Count; i++)
            {
                if (RecursivelyFillInNextNumberOptimised(sudoku, nextCoord.Row, nextCoord.Col, i))
                {
                    return true;
                }
            }
            sudoku.Squares[row, col] = startingValue;
            return false;
        }

        public Coord FindNextSquare(Sudoku sudoku)
        {
            var coordsToFillIn = new List<Coord>();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (sudoku.Squares[row, col] == 0)
                    {
                        coordsToFillIn.Add(new Coord(row, col));
                    }
                }
            }
            var possibleNumbers = new Dictionary<Coord, List<int>>();
            foreach (var coord in coordsToFillIn)
            {
                possibleNumbers[coord] = FindAvailableNumbers(sudoku, coord.Row, coord.Col);
            }
            return possibleNumbers.OrderBy(c => c.Value.Count).First().Key;
        }

        public bool IsFinished(Sudoku sudoku)
        {
            foreach (var square in sudoku.Squares)
            {
                if (square == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public List<int> FindAvailableNumbers(Sudoku sudoku, int thisRow, int thisCol)
        {
            if (sudoku.Squares[thisRow, thisCol] != 0)
            {
                return new List<int> { sudoku.Squares[thisRow, thisCol] };
            }
            var availableNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            availableNumbers = CheckRows(availableNumbers, sudoku, thisRow);
            availableNumbers = CheckColumns(availableNumbers, sudoku, thisCol);
            availableNumbers = CheckBox(availableNumbers, sudoku, thisRow, thisCol);
            return availableNumbers;
        }

        public List<int> CheckRows(List<int> availableNumbers, Sudoku sudoku, int row)
        {
            for (int col = 0; col < 9; col++)
            {
                availableNumbers.Remove(sudoku.Squares[row, col]);
            }
            return availableNumbers;
        }

        public List<int> CheckColumns(List<int> availableNumbers, Sudoku sudoku,int col)
        {
            for (int row = 0; row < 9; row++)
            {
                availableNumbers.Remove(sudoku.Squares[row, col]);
            }
            return availableNumbers;
        }

        public List<int> CheckBox(List<int> availableNumbers, Sudoku sudoku, int thisRow, int thisCol)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var rowToCheck = 3 * (thisRow / 3) + row;
                    var colToCheck = 3 * (thisCol / 3) + col;
                    availableNumbers.Remove(sudoku.Squares[rowToCheck, colToCheck]);
                }
            }
            return availableNumbers;
        }
    }

    public class Coord
    {
        public int Row { get; }
        public int Col { get; }

        public Coord(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}
