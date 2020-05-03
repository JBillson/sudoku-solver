using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SudokuSolver : MonoBehaviour
{
    private SudokuController _sudokuController;
    private List<Cell> _cells;

    private void Awake()
    {
        _sudokuController = FindObjectOfType<SudokuController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Solve();
    }

    private void Solve()
    {
        if (!_sudokuController.HasStartedPuzzle())
        {
            Debug.Log("Cannot Solve Puzzle before you start it");
            return;
        }

        Debug.Log("Starting Solve");
        _cells = _sudokuController.sudokuInstance.cells;
        ContinueSolve(0);
    }

    private void ContinueSolve(int pos)
    {
        for (int i = pos; i < _cells.Count; i++)
        {
            // Continue if this cell already has a value
            if (_cells[i].value != 0 || !_cells[i].editable) continue;

            Debug.Log($"Solving Cell {_cells[i].row}:{_cells[i].column}");

            // Try each value for this empty cell
            var numbers = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            foreach (var number in numbers)
            {
                // Continue to the next number if it is invalid
                if (!_sudokuController.InputValue(_cells[i].cellView, number)) continue;

                // Render the Cell once the new Cell value has been set
                _cells[i].cellView.RenderCell();
                Debug.Log($"{number} input");
                break;
            }

            // Backtrack if the Value in the Cell is still 0 at this point
            if (_cells[i].value != 0) continue;
            Debug.Log($"Cell {_cells[i].row}:{_cells[i].column} has no valid option\nStart Backtracking");
            Backtrack(i - 1);
            break;
        }
    }

    private void Backtrack(int cellPos)
    {
        while (true)
        {
            // Get the cell we have backtracked to
            var cellToEdit = _cells[cellPos];

            // Backtrack again if the cell is not editable
            if (!cellToEdit.editable)
            {
                Debug.Log(
                    $"Cell {_cells[cellPos].row}:{_cells[cellPos].column} is not editable.  Backtracking again");
                cellPos -= 1;
                continue;
            }

            Debug.Log($"Backtracked to Cell {cellToEdit.row}:{cellToEdit.column}");

            // Continue trying values higher than the current value of the cell we are editing
            for (var j = 1; j <= 9; j++)
            {
                if (j <= cellToEdit.value) continue;
                if (!_sudokuController.InputValue(cellToEdit.cellView, j)) continue;

                // Render the cell and continue solving
                cellToEdit.cellView.RenderCell();
                Debug.Log($"{j} input in Cell {cellToEdit.row}:{cellToEdit.column}");
                ContinueSolve(cellPos + 1);
                return;
            }

            // If there is no other valid option for this cell
            // Reset cell and backtrack again
            cellToEdit.cellView.SelectNumber(0);
            Debug.Log(
                $"No Valid backtrack input for Cell {cellToEdit.row}:{cellToEdit.column}. Backtracking again");
            cellPos -= 1;
        }
    }
}