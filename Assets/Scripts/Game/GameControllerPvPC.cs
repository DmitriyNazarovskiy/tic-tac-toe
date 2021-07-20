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

		public GameControllerPvPC(ICommonFactory factory, GameConfig config, ITimerController timerController,
			Action showMainMenu, GameMode mode) : base(factory, config, timerController, showMainMenu, mode)
			=> _pcDelayEntityId = timerController.CreateTimeEntity();

		protected async void SetRandomMark()
		{
			var clearCells = Cells.ToList().FindAll(c => c.Model.CurrentState == CellState.Clear);
			var randomCell = clearCells.RandomElement();
			var timer = 0.0f;

			TimerController.ResetTimeEntity(_pcDelayEntityId);

			while (timer < 1)
			{
				timer = TimerController.ElapsedTime(_pcDelayEntityId);

				await Task.Delay(1);
			}

			OnCellClick(randomCell.GetCellId());
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
