using Configs;
using Core;
using UnityEngine;

namespace Game
{
	public class GameControllerPvP : GameControllerBase
	{
		public GameControllerPvP(ICommonFactory factory, Transform canvas, GameConfig config, ITimerController timerController) : base(factory, canvas, config, timerController)
		{
		}
	}
}
