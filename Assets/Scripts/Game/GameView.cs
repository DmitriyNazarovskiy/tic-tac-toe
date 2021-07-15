using Game.Cell;
using UnityEngine;

namespace Game
{
	public class GameView : MonoBehaviour
	{
		[SerializeField] private CellView[] _cells;

		public CellView[] GetCells() => _cells;
	}
}
