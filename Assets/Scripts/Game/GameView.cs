using Game.Cell;
using TMPro;
using UnityEngine;

namespace Game
{
	public class GameView : MonoBehaviour
	{
		[SerializeField] private CellView[] _cells;
		[SerializeField] private TMP_Text _timerValue;

		public CellView[] GetCells() => _cells;
		public void SetTimer(int value) => _timerValue.text = value.ToString();
		public void SetTimerColor(Color color) => _timerValue.color = color;
	}
}
