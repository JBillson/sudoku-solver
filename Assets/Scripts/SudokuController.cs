using UnityEngine;

public class SudokuController : MonoBehaviour
{
    private const string DummyData =
        "106409307003008400000026000000000506015000790809000000000960000004100800701204603";

    public Sudoku sudokuInstance;
    private SudokuView _sudokuView;
    private bool _hasStartedPuzzle;
    [HideInInspector] public bool isSelectingNumber;

    private void Awake()
    {
        _sudokuView = GetComponent<SudokuView>();
    }

    private void Start()
    {
        CreateLayout();
    }

    private void CreateLayout()
    {
        sudokuInstance = new Sudoku();
        _hasStartedPuzzle = false;
        _sudokuView.RenderPuzzle();
    }

    public void ClearPuzzle()
    {
        sudokuInstance = null;
        _sudokuView.ClearPuzzle();
        CreateLayout();
    }

    public void CreateNewSudoku()
    {
        _hasStartedPuzzle = true;
        _sudokuView.ClearPuzzle();
        sudokuInstance.SetupBoard(DummyData);
        _sudokuView.RenderPuzzle();
    }

    public bool HasStartedPuzzle()
    {
        return _hasStartedPuzzle;
    }

    public bool InputValue(CellView cellView, int value)
    {
        if (!sudokuInstance.IsValidInput(cellView.cell, value)) return false;
        cellView.cell.value = value;
        return true;
    }
}