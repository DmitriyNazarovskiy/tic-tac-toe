using System;
using System.Linq;
using Configs;
using Core;
using Game.Cell;
using Game.ResultPopup;
using UnityEngine;

namespace Game
{
	public abstract class GameControllerBase : IUpdatable, IClearable
	{
		private readonly GameConfig _config;
		private readonly ITimerController _timerController;
		private readonly ICommonFactory _factory;
		private readonly int _timerEntityId;
		private readonly Action _showMainMenuAction;

		protected GameModel Model;
		protected GameView View;

		private CellController[] _cells;
		private ResultPopupController _resultPopup;

		protected GameControllerBase(ICommonFactory factory, Transform canvas, GameConfig config, ITimerController timerController, Action showMainMenu )
		{
			_factory = factory;
			_config = config;
			_timerController = timerController;
			_showMainMenuAction = showMainMenu;

			_timerEntityId = _timerController.CreateTimeEntity();

			Model = new GameModel(config);

			CreateGameView(_factory, canvas, config.GameViewPrefab);

			StartGame();
		}

		private void CreateGameView(ICommonFactory factory, Transform canvas, GameObject gamePrefab)
			=> View = factory.InstantiateObject<GameView>(gamePrefab, canvas);

		private void InitCells()
		{
			_cells = new CellController[Constants.CellsAmount];

			var views = View.GetCells();

			for (var i = 0; i < _cells.Length; i++)
			{
				_cells[i] = new CellController(views[i], OnCellClick);
			}
		}

		private void StartGame()
		{
			View.SetTimer(_config.GameDuration);
			View.SetTimerColor(_config.DefaultTimerColor);
			View.SetTurn(Model.GetCurrentPlayerLabel());

			InitCells();

			Model.GameInProgress = true;

			_timerController.ResetTimeEntity(_timerEntityId);
		}

		private void OnCellClick(byte id)
		{
			if (!Model.GameInProgress)
				return;

			Debug.Log("Clicked " + id);

			var cell = _cells.First(c => c.GetCellId() == id);
			cell.SetCellState(Model.CurrentTurnState, Model.GetCellSprite(Model.CurrentTurnState));

			Model.MarkedCells.Add(cell.Model);

			var result = Model.CheckGameResult();

			switch (result)
			{
				case GameResult.None:
					break;
				case GameResult.Player1Win:
					GameFinished(GameResult.Player1Win);
					break;
				case GameResult.Player2Win:
					GameFinished(GameResult.Player2Win);
					break;
				case GameResult.Draw:
					GameFinished(GameResult.Draw);
					break;
				case GameResult.TimeOver:
					GameFinished(GameResult.TimeOver);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if(result == GameResult.None)
				SwitchTurn();
		}

		public void Update(float deltaTime)
		{
			if(!Model.GameInProgress)
				return;

			TimerUpdate();
		}

		private void TimerUpdate()
		{
			var timerValue = (int)_timerController.ElapsedTimeReverse(_timerEntityId, _config.GameDuration);

			if(timerValue < Constants.RedTimerValue)
				View.SetTimerColor(_config.LowTimerColor);

			if (timerValue == 0)
			{
				GameFinished(GameResult.TimeOver);
			}

			View.SetTimer(timerValue);
		}

		private void SwitchTurn()
		{
			Model.CurrentTurnState = Model.CurrentTurnState == CellState.X ? CellState.O : CellState.X;

			View.SetTurn(Model.GetCurrentPlayerLabel());
		}

		private void GameFinished(GameResult result)
		{
			Debug.Log(result); 

			Model.GameInProgress = false;

			_resultPopup = new ResultPopupController();
			_resultPopup.CreateView(_factory, _config.ResultPopupPrefab, View.GetUiParent());
			_resultPopup.SetResultMessage(result);
			_resultPopup.InitButtons(Restart, GoToMenu);
		}

		private void GoToMenu()
		{
			_resultPopup.Clear();

			_showMainMenuAction?.Invoke();
		}

		private void Restart()
		{
			_resultPopup.Clear();

			Model.MarkedCells.Clear();
			Model.CurrentTurnState = CellState.X;

			StartGame();
		}

		public void Clear()
		{
			View.Clear();
			View = null;
		}
	}
}
