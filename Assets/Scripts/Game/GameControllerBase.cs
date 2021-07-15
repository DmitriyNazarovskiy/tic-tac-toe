using Core;
using Core.Raycast;
using Game.Cell;
using UnityEngine;

namespace Game
{
	public abstract class GameControllerBase : IUpdatable
	{
		protected GameModel Model;
		protected GameView View;

		private CellController[] _cells;

		protected GameControllerBase(ICommonFactory factory, Transform canvas, GameObject gamePrefab)
		{
			Model = new GameModel();

			CreateGameView(factory, canvas, gamePrefab);

			InitCells();
		}

		private void InitCells()
		{
			_cells = new CellController[Constants.CellsAmount];

			var views = View.GetCells();

			for (var i = 0; i < _cells.Length; i++)
			{
				_cells[i] = new CellController(views[i], OnCellClick);
			}
		}

		private void CreateGameView(ICommonFactory factory, Transform canvas, GameObject gamePrefab)
		{
			View = factory.InstantiateObject<GameView>(gamePrefab, canvas);
		}

		private void OnCellClick(byte id)
		{
			Debug.Log("Clicked " + id);
		}

		public void Update(float deltaTime)
		{
			
		}
	}
}
