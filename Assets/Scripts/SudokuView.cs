using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SudokuController))]
public class SudokuView : MonoBehaviour
{
    public CellView cellPrefab;
    public Transform cellHolder;
    private SudokuController _sudokuController;
    private readonly List<CellView> _listOfCells = new List<CellView>();

    private void Awake()
    {
        _sudokuController = GetComponent<SudokuController>();
    }

    public void RenderPuzzle()
    {
        foreach (var cell in _sudokuController.sudokuInstance.cells)
        {
            var cellView = Instantiate(cellPrefab, cellHolder);
            _listOfCells.Add(cellView);
            cellView.Init(cell);
            cellView.RenderCell();
        }
    }

    public void ClearPuzzle()
    {
        if (_listOfCells.Count <= 0) return;
        foreach (var cellView in _listOfCells)
        {
            Destroy(cellView.gameObject);
        }

        _listOfCells.Clear();
    }
}