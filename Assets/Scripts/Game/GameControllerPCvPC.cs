using System;
using Configs;
using Core;

namespace Game
{
	public class GameControllerPCvPC : GameControllerPvPC
	{
		public GameControllerPCvPC(ICommonFactory factory, GameConfig config, ITimerController timerController,
			Action showMainMenu, GameMode mode)
			: base(factory, config, timerController, showMainMenu, mode)
		{
		}

		protected override void DoPcTurn() => SetRandomMark();

		protected override void StartGame()
		{
			base.StartGame();

			SetRandomMark();
		}
	}
}
