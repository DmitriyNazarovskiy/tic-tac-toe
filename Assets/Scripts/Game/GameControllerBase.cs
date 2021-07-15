using Core;
using UnityEngine;

namespace Game
{
	public abstract class GameControllerBase
	{
		protected GameModel Model;
		protected GameView View;

		protected GameControllerBase()
		{
			Model = new GameModel();
		}

		public void CreateGameView(ICommonFactory factory, Transform canvas, GameObject gamePrefab)
		{
			View = factory.InstantiateObject<GameView>(gamePrefab, canvas);
		}
	}
}
