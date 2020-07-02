using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

public class SudokuSolver : MonoBehaviour
{
    private SudokuController _sudokuController;
    private List<Cell> _cells;
    private Stopwatch _sw;
    private int _timesBacktracked;
    private int _currentCellPos;

    private void Awake()
    {
        _sudokuController = FindObjectOfType<SudokuController>();
    }

    public void Solve()
    {
        if (!_sudokuController.HasStartedPuzzle()) return;

        _sw = new Stopwatch();
        _sw.Start();
        _timesBacktracked = 0;

        _cells = _sudokuController.sudokuInstance.cells;
        // Task.Run(() => ContinueSolve(0));
        ContinueSolve(0);
    }

    private void ContinueSolve(int pos)
    {
        print("Continuing the Solve");
        for (var i = pos; i < _cells.Count; i++)
        {
            // Continue if this cell already has a value
            if (_cells[i].value != 0 || !_cells[i].editable)
            {
                Debug.Log($"Skipping Cell {_cells[i].row}:{_cells[i].column} - it is not editable.");
                continue;
            }

            Debug.Log($"Solving Cell {_cells[i].row}:{_cells[i].column}");
            _currentCellPos = i;

            // Try each value for this empty cell
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            if (numbers.Any(number => _sudokuController.InputValue(_cells[i].cellView, number)))
            {
                // UnityMainThreadDispatcher.Instance().Enqueue(() => _cells[i].cellView.RenderCell());
                _cells[i].cellView.RenderCell();
                Debug.Log($"{_cells[i].value} input in Cell {_cells[i].row}:{_cells[i].column}");
            }

            // Backtrack if the Value in the Cell is still 0 at this point
            if (_cells[i].value != 0) continue;
            Debug.Log($"Cell {_cells[i].row}:{_cells[i].column} has no valid option\nStart Backtracking");
            Backtrack();
            break;
        }

        _sw.Stop();
        Debug.Log(
            $"Completed the Sudoku in {_sw.ElapsedMilliseconds / 1000f} seconds\n" +
            $"Times Backtracked: {_timesBacktracked}");
    }

    private void Backtrack()
    {
        while (true)
        {
            // Backtrack one cell
            _currentCellPos--;
            _timesBacktracked++;

            // Get the cell we have backtracked to
            var cellToEdit = _cells[_currentCellPos];

            // Backtrack again if the cell is not editable
            if (!cellToEdit.editable)
            {
                Debug.Log(
                    $"Cell {_cells[_currentCellPos].row}:{_cells[_currentCellPos].column} is not editable.  Backtracking again");
                continue;
            }

            Debug.Log($"Backtracked to Cell {cellToEdit.row}:{cellToEdit.column}");

            // Continue trying values higher than the current value of the cell we are editing
            for (var j = 1; j <= 9; j++)
            {
                if (j <= cellToEdit.value) continue;
                if (!_sudokuController.InputValue(cellToEdit.cellView, j)) continue;

                // Render the cell and continue solving
                // UnityMainThreadDispatcher.Instance().Enqueue(() => cellToEdit.cellView.RenderCell());
                cellToEdit.cellView.RenderCell();
                Debug.Log($"{j} input in Cell {cellToEdit.row}:{cellToEdit.column}");
                goto ContinueTheSolve;
            }

            // If there is no other valid option for this cell
            // Reset cell and backtrack again
            cellToEdit.cellView.SelectNumber(0);
            Debug.Log(
                $"No Valid backtrack input for Cell {cellToEdit.row}:{cellToEdit.column}. Backtracking again");
        }

    ContinueTheSolve:
        {
            ContinueSolve(_currentCellPos + 1);
        }
    }
}