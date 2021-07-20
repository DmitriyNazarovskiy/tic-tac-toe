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

			SetHints(true);
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

			var clearCells = Cells.ToList().FindAll(c => c.Model.CurrentState == CellState.Clear);

			foreach (var cell in clearCells)
				cell.SetHint(true, Config.HintMaterial);
		}

		protected override void OnCellClick(byte id)
		{
			base.OnCellClick(id);

			SetHints(false);
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

			OnCellClick(randomCell.GetCellId());

			TimerController.ResetTimeEntity(_hintDelayEntityId);

			SetHints(true);
		}

		protected override void SwitchTurn()
		{
			base.SwitchTurn();

			DoPcTurn();
		}

		protected virtual void DoPcTurn()
		{
			if (Model.CurrentTurnState == CellState.O)
				SetRandomMark();
		}

		public override void Clear()
		{
			base.Clear();

			TimerController.RemoveEntity(_pcDelayEntityId);
		}
	}
}
