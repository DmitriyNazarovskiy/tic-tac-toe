using System;
using Core;
using Game.Cell;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class GameView : MonoBehaviour, IClearable
	{
		[SerializeField] private CellView[] _cells;
		[SerializeField] private TMP_Text _timerValue, _currentTurnValue;
		[SerializeField] private Transform _uiTransform;
		[SerializeField] private Button _undoButton;

		public void InitUndoButton(Action undoAction)
		{
			_undoButton.gameObject.SetActive(true);
			_undoButton.onClick.AddListener(() => undoAction?.Invoke());
		}

		public void SetUndoButtonClickability(bool isClickable) => _undoButton.interactable = isClickable;
		public CellView[] GetCells() => _cells;
		public void SetTimer(int value) => _timerValue.text = value.ToString();
		public void SetTimerColor(Color color) => _timerValue.color = color;
		public void SetTurn(string value) => _currentTurnValue.text = value;
		public Transform GetUiParent() => _uiTransform;
		
		public void Clear()
		{
			_undoButton.onClick.RemoveAllListeners();

			Destroy(gameObject);
		}
	}
}
