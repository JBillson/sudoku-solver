using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sudoku
{
    public readonly List<Cell> cells = new List<Cell>();

    public Sudoku()
    {
        cells.Clear();

        for (var r = 0; r < 9; r++)
        {
            for (var c = 0; c < 9; c++)
            {
                var newCell = new Cell()
                {
                    column = c + 1,
                    row = r + 1,
                    value = 0
                };
                cells.Add(newCell);
            }
        }
    }

    public void SetupBoard(string data)
    {
        for (var r = 0; r < 9; r++)
        {
            var dataForRow = data.Substring(0, 9);
            var column = 0;
            foreach (var cell in cells.Where(cell => cell.row == r + 1))
            {
                column++;
                cell.value = int.Parse(dataForRow[column - 1].ToString());
            }

            data = data.Remove(0, 9);
        }
    }

    public bool IsValidInput(Cell cell, int value)
    {
        var isColumnCollision = CheckColumnCollision(cell, value);
        var isRowCollision = CheckRowCollision(cell, value);
        var isBlockCollision = CheckBlockCollision(cell, value);

        if (isBlockCollision)
            Debug.Log("Block Collision");
        if (isRowCollision)
            Debug.Log("Row Collision");
        if (isColumnCollision)
            Debug.Log("Column Collision");

        return !isColumnCollision && !isRowCollision && !isBlockCollision;
    }

    private bool CheckColumnCollision(Cell currentCell, int value)
    {
        if (value == 0) return false;
        var cellsInSameColumn =
            cells.Where(cell => cell.column == currentCell.column && cell.row != currentCell.row).ToList();
        return cellsInSameColumn.Any(cell => cell.value == value);
    }

    private bool CheckRowCollision(Cell currentCell, int value)
    {
        if (value == 0) return false;
        var cellsInSameRow =
            cells.Where(cell => cell.row == currentCell.row && cell.column != currentCell.column).ToList();
        return cellsInSameRow.Any(cell => cell.value == value);
    }

    private bool CheckBlockCollision(Cell currentCell, int value)
    {
        if (value == 0) return false;
        var rowToCountFrom = 0;

        if (currentCell.row <= 3)
            rowToCountFrom = 1;
        else if (currentCell.row > 3 && currentCell.row <= 6)
            rowToCountFrom = 4;
        else if (currentCell.row > 6)
            rowToCountFrom = 7;

        var columnToCountFrom = 0;
        if (currentCell.column <= 3)
            columnToCountFrom = 1;
        else if (currentCell.column > 3 && currentCell.column <= 6)
            columnToCountFrom = 4;
        else if (currentCell.column > 6)
            columnToCountFrom = 7;

        for (var r = rowToCountFrom; r < rowToCountFrom + 3; r++)
        {
            for (var c = columnToCountFrom; c < columnToCountFrom + 3; c++)
            {
                var cell = GetCellFromRowAndColumnNumber(r, c);
                if (cell.value == value && r != currentCell.row && c != currentCell.column) return true;
            }
        }

        return false;
    }


    private Cell GetCellFromRowAndColumnNumber(int row, int column)
    {
        return cells.First(cell => cell.row == row && cell.column == column);
    }

    public void PrintBoard()
    {
        var outputText = "---Sudoku Board---\n";
        for (var i = 0; i < 15; i++)
        {
            foreach (var cell in cells.Where(cell => cell.row == i))
            {
                outputText += $"{cell.value}  ";
            }

            outputText += "\n";
        }

        Debug.Log(outputText);
    }
}