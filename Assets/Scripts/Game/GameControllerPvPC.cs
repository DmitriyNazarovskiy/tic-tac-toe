using System;
using System.Linq;
using System.Threading.Tasks;
using Configs;
using Core;

namespace Game
{
	public class GameControllerPvPC : GameControllerBase
	{
		private readonly int _pcDelayEntityId;
		private readonly int _hintDelayEntityId;

		public GameControllerPvPC(ICommonFactory factory, GameConfig config, ITimerController timerController,
			Action showMainMenu, GameMode mode) : base(factory, config, timerController, showMainMenu, mode)
		{
			_pcDelayEntityId = timerController.CreateTimeEntity();
			_hintDelayEntityId = timerController.CreateTimeEntity();
		}

		protected override void StartGame()
		{
			base.StartGame();

			View.InitUndoButton(UndoButtonPressed);
			View.SetUndoButtonClickability(false);

			TimerController.ResetTimeEntity(_hintDelayEntityId);

			ResetHints();
		}

		private async void SetHints(bool isShow)
		{
			if (!isShow)
			{
				foreach (var cell in Cells)
					cell.SetHint(false);

				return;
			}

			while (TimerController.ElapsedTime(_hintDelayEntityId) < Constants.HintsDelay)
				await Task.Delay(1);

			if (!Model.GameInProgress)
				return;

			var clearCells = Cells.ToList().FindAll(c => c.Model.CurrentState == CellState.Clear);

			foreach (var cell in clearCells)
				cell.SetHint(true, Config.HintMaterial);
		}

		private void ResetHints()
		{
			SetHints(false);
			SetHints(true);
		}

		protected async void SetRandomMark()
		{
			var clearCells = Cells.ToList().FindAll(c => c.Model.CurrentState == CellState.Clear);
			var randomCell = clearCells.RandomElement();
			var timer = 0.0f;

			TimerController.ResetTimeEntity(_pcDelayEntityId);

			while (timer < Constants.PcTurnDelay)
			{
				timer = TimerController.ElapsedTime(_pcDelayEntityId);

				await Task.Delay(1);
			}

			base.OnCellClick(randomCell.GetCellId());

			TimerController.ResetTimeEntity(_hintDelayEntityId);

			SetHints(true);
		}

		private void UndoButtonPressed()
		{
			if (Model.MarkedCells.Count < 2)
			{
				View.SetUndoButtonClickability(false);

				return;
			}

			var lastMoves = new[]
			{
				Model.MarkedCells[Model.MarkedCells.Count - 1],
				Model.MarkedCells[Model.MarkedCells.Count - 2]
			};

			foreach (var cell in lastMoves)
			{
				cell.CurrentState = CellState.Clear;
				Cells.First(c => c.GetCellId() == cell.Id).ClearCell();
				Model.MarkedCells.Remove(cell);
			}

			ResetHints();
		}

		protected override void SwitchTurn()
		{
			base.SwitchTurn();

			SetHints(false);

			DoPcTurn();

			View.SetUndoButtonClickability(Model.CurrentTurnState == CellState.X);
		}

		protected override void GameFinished(GameResult result)
		{
			base.GameFinished(result);

			SetHints(false);
		}

		protected virtual void DoPcTurn()
		{
			if (Model.CurrentTurnState == CellState.O)
				SetRandomMark();

			CheckUndoButton();
		}

		private void CheckUndoButton()
		{
			if(Model.MarkedCells.Count > 1)
				View.SetUndoButtonClickability(true);
		}

		protected override void OnCellClick(byte id)
		{
			if (Model.CurrentTurnState == CellState.O)
				return;

			base.OnCellClick(id);
		}

		public override void Clear()
		{
			base.Clear();

			TimerController.RemoveEntity(_pcDelayEntityId);
			TimerController.RemoveEntity(_hintDelayEntityId);
		}
	}
}
