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
		protected readonly ITimerController TimerController;

		private readonly GameConfig _config;
		private readonly ICommonFactory _factory;
		private readonly int _timerEntityId;
		private readonly Action _showMainMenuAction;

		protected GameModel Model;
		protected GameView View;

		protected CellController[] Cells;
		private ResultPopupController _resultPopup;

		protected GameControllerBase(ICommonFactory factory, GameConfig config, ITimerController timerController, Action showMainMenu, GameMode mode)
		{
			_factory = factory;
			_config = config;
			TimerController = timerController;
			_showMainMenuAction = showMainMenu;

			_timerEntityId = TimerController.CreateTimeEntity();

			Model = new GameModel(config, mode);
		}

		public void CreateGameView(Transform canvas, GameObject gamePrefab)
		{
			View = _factory.InstantiateObject<GameView>(gamePrefab, canvas);

			StartGame();
		}

		private void InitCells()
		{
			Cells = new CellController[Constants.CellsAmount];

			var views = View.GetCells();

			for (var i = 0; i < Cells.Length; i++)
			{
				Cells[i] = new CellController(views[i], OnCellClick);
			}
		}

		private void StartGame()
		{
			View.SetTimer(_config.GameDuration);
			View.SetTimerColor(_config.DefaultTimerColor);
			View.SetTurn(Model.GetCurrentPlayerLabel());

			InitCells();

			Model.GameInProgress = true;

			TimerController.ResetTimeEntity(_timerEntityId);
		}

		protected void OnCellClick(byte id)
		{
			if (!Model.GameInProgress)
				return;

			Debug.Log("Clicked " + id);

			var cell = Cells.First(c => c.GetCellId() == id);
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
			var timerValue = (int)TimerController.ElapsedTimeReverse(_timerEntityId, _config.GameDuration);

			if(timerValue < Constants.RedTimerValue)
				View.SetTimerColor(_config.LowTimerColor);

			if (timerValue == 0)
			{
				GameFinished(GameResult.TimeOver);
			}

			View.SetTimer(timerValue);
		}

		protected virtual void SwitchTurn()
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
			_resultPopup.SetResultMessage(result, Model.GameMode);
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

		public virtual void Clear()
		{
			TimerController.RemoveEntity(_timerEntityId);

			View.Clear();
			View = null;
		}
	}
}
