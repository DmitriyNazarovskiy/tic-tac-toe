﻿using System;
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
		protected readonly GameConfig Config;

		private readonly ICommonFactory _factory;
		private readonly int _gameTimerEntityId;
		private readonly Action _showMainMenuAction;

		public GameView View { get; private set; }
		public GameModel Model { get; }

		protected CellController ClickedCell;
		protected CellController[] Cells;

		private ResultPopupController _resultPopup;

		protected GameControllerBase(ICommonFactory factory, GameConfig config, ITimerController timerController,
			Action showMainMenu, GameMode mode)
		{
			_factory = factory;
			Config = config;
			TimerController = timerController;
			_showMainMenuAction = showMainMenu;

			_gameTimerEntityId = TimerController.CreateTimeEntity();

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
				if (Model.GameMode == GameMode.PcVsPc)
					Cells[i] = new CellController(views[i]);
				else
					Cells[i] = new CellController(views[i], OnCellClick);
			}
		}

		protected virtual void StartGame() => InitGameStart();

		protected void InitGameStart()
		{
			View.SetTimer(Config.GameDuration);
			View.SetTimerColor(Config.DefaultTimerColor);
			View.SetTurn(Model.GetCurrentPlayerLabel());

			InitCells();

			Model.GameInProgress = true;

			TimerController.ResetTimeEntity(_gameTimerEntityId);
		}

		protected virtual void OnCellClick(byte id)
		{
			if (!Model.GameInProgress)
				return;

			ClickedCell = Cells.First(c => c.GetCellId() == id);

			if (ClickedCell.Model.CurrentState != CellState.Clear)
			{
				AudioPlayer.PlayEffect(Config.AudioConfig.WrongTapSound, Constants.DefaultSoundVolume);

				return;
			}

			AudioPlayer.PlayEffect(Config.AudioConfig.TapSound, Constants.DefaultSoundVolume,
				pitch: Model.CurrentTurnState == CellState.X ? Constants.DefaultPitch : Constants.CircleMovePitch);

			ClickedCell.SetCellState(Model.CurrentTurnState, Model.GetCellSprite(Model.CurrentTurnState));

			Model.MarkedCells.Add(ClickedCell.Model);

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

			if (result == GameResult.None)
				SwitchTurn();
		}

		public void Update(float deltaTime)
		{
			if (!Model.GameInProgress)
				return;

			TimerUpdate();
		}

		private void TimerUpdate()
		{
			var timerValue = (int) TimerController.ElapsedTimeReverse(_gameTimerEntityId, Config.GameDuration);

			if (timerValue < Constants.RedTimerValue)
				View.SetTimerColor(Config.LowTimerColor);

			if (timerValue == 0)
				GameFinished(GameResult.TimeOver);

			View.SetTimer(timerValue);
		}

		protected virtual void SwitchTurn()
		{
			Model.CurrentTurnState = Model.CurrentTurnState == CellState.X ? CellState.O : CellState.X;

			View.SetTurn(Model.GetCurrentPlayerLabel());
		}

		protected virtual void GameFinished(GameResult result)
		{
			AudioPlayer.PlayEffect(Config.AudioConfig.GameFinished, Constants.DefaultSoundVolume);

			Model.GameInProgress = false;

			_resultPopup = new ResultPopupController();
			_resultPopup.CreateView(_factory, Config.ResultPopupPrefab, View.GetUiParent());
			_resultPopup.SetResultMessage(result, Model.GameMode);
			_resultPopup.InitButtons(Restart, GoToMenu);
		}

		private void GoToMenu()
		{
			AudioPlayer.PlayEffect(Config.AudioConfig.TapSound, Constants.DefaultSoundVolume);

			_resultPopup.Clear();

			_showMainMenuAction?.Invoke();
		}

		private void Restart()
		{
			AudioPlayer.PlayEffect(Config.AudioConfig.TapSound, Constants.DefaultSoundVolume);

			_resultPopup.Clear();

			Model.MarkedCells.Clear();
			Model.CurrentTurnState = CellState.X;

			StartGame();
		}

		public virtual void Clear()
		{
			TimerController.RemoveEntity(_gameTimerEntityId);

			View.Clear();
			View = null;
		}
	}
}
