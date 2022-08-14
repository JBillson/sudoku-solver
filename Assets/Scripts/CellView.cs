using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CellView : MonoBehaviour
{
    public GameObject numberSelector;
    [HideInInspector] public Cell cell;
    private TextMeshProUGUI _text;
    private Button _cellButton;
    private SudokuController _sudokuController;
    private List<Button> _numberButtons = new List<Button>();

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _cellButton = GetComponentInChildren<Button>();
        _sudokuController = FindObjectOfType<SudokuController>();
    }

    public void Init(Cell newCell)
    {
        cell = newCell;
        if (_sudokuController.HasStartedPuzzle())
            _cellButton.onClick.AddListener(EnterNumberSelect);

        _numberButtons = numberSelector.GetComponentsInChildren<Button>().ToList();
        for (var i = 0; i < _numberButtons.Count; i++)
        {
            var number = i;
            _numberButtons[number].onClick.AddListener(() => SelectNumber(number));
        }

        RenderCell();
    }

    private void EnterNumberSelect()
    {
        if (_sudokuController.isSelectingNumber) return;
        _sudokuController.isSelectingNumber = true;
        numberSelector.SetActive(true);
    }

    public void SelectNumber(int number)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            _sudokuController.isSelectingNumber = false;

            var valid = _sudokuController.InputValue(this, number);
            numberSelector.SetActive(false);
            cell.value = number;
            RenderCell(valid);
        });
    }

    public void RenderCell()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            _text.text = cell.value == 0 ? "" : cell.value.ToString();
            _cellButton.image.color = Color.white;
        });
    }

    private void RenderCell(bool valid)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            _text.text = cell.value == 0 ? "" : cell.value.ToString();
            _cellButton.image.color = valid ? Color.white : Color.red;
        });
    }
}