using System.Linq;
using Configs;
using Core;
using Game.Cell;
using UnityEngine;

namespace Game
{
	public abstract class GameControllerBase : IUpdatable
	{
		private readonly GameConfig _config;
		private readonly ITimerController _timerController;
		private readonly int _timerEntityId;

		protected GameModel Model;
		protected GameView View;

		private CellController[] _cells;

		protected GameControllerBase(ICommonFactory factory, Transform canvas, GameConfig config, ITimerController timerController)
		{
			_config = config;
			_timerController = timerController;

			_timerEntityId = _timerController.CreateTimeEntity();

			Model = new GameModel(config);

			CreateGameView(factory, canvas, config.GameViewPrefab);

			View.SetTimer(_config.GameDuration);
			View.SetTimerColor(_config.DefaultTimerColor);

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

			var cell = _cells.First(c => c.GetCellId() == id);
			cell.SetCellState(Model.CurrentTurnState, Model.GetCellSprite(Model.CurrentTurnState));

			Model.MarkedCells.Add(cell.Model);

			var result = Model.CheckGameResult();

			SwitchTurn();
		}

		public void Update(float deltaTime)
		{
			TimerUpdate();
		}

		private void TimerUpdate()
		{
			var timerValue = (int)_timerController.ElapsedTimeReverse(_timerEntityId, _config.GameDuration);

			if(timerValue < Constants.RedTimerValue)
				View.SetTimerColor(_config.LowTimerColor);

			if (timerValue < 0)
			{
				timerValue = 0;
			}

			View.SetTimer(timerValue);
		}

		private void SwitchTurn()
			=> Model.CurrentTurnState = Model.CurrentTurnState == CellState.X ? CellState.O : CellState.X;
	}
}
