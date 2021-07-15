using Configs;
using Game;
using Menu;
using UnityEngine;

namespace Core
{
	public class MainController : MonoBehaviour
	{
		[SerializeField] private GameConfig _config;
		[SerializeField] private Transform _canvasTransform;

		private ICommonFactory _commonFactory;
		private MenuController _menuController;
		private GameControllerBase _gameController;

		private void Awake()
		{
			_commonFactory = new CommonFactory();

			InitControllers();
		}

		private void InitControllers()
		{
			_menuController = new MenuController(_commonFactory, _config.MainMenuPrefab, _canvasTransform, StartGame);
		}

		private void StartGame(int modeId)
		{
			switch (modeId)
			{
				case 1:
					_gameController = new GameControllerPvP();
					break;
				default:
					Debug.Log("error");
					break;
			}

			_menuController?.Clear();

			_gameController.CreateGameView(_commonFactory, _canvasTransform, _config.GameViewPrefab);
		}
	}
}
