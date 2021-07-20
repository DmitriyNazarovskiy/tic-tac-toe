using System;
using Configs;
using Core;

namespace Game
{
	public class GameControllerPvP : GameControllerBase
	{
		public GameControllerPvP(ICommonFactory factory, GameConfig config, ITimerController timerController, Action showMainMenu, GameMode mode)
			: base(factory, config, timerController, showMainMenu, mode)
		{
		}
	}
}
