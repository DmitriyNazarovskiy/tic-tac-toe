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
		[SerializeField] private Camera _camera;

		private ICommonFactory _commonFactory;
		private MenuController _menuController;
		private GameControllerBase _gameController;
		private ITimerController _timerController;

		private void Awake()
		{
			_commonFactory = new CommonFactory();

			InitControllers();

			ShowMainMenu();
		}

		private void InitControllers()
		{
			_menuController = new MenuController();
			_timerController = new TimerController();
		}

		private void ShowMainMenu()
		{
			_menuController.CreateView(_commonFactory, _config.MainMenuPrefab, _canvasTransform, StartGame);

			_gameController?.Clear();
			_gameController = null;
		}

		private void Update()
		{
			if(_gameController == null)
				return;

			var deltaTime = Time.deltaTime;

			_gameController?.Update(deltaTime);
		}

		private void StartGame(int modeId)
		{
			switch (modeId)
			{
				case 1:
					_gameController = new GameControllerPvP(_commonFactory, _canvasTransform, _config, _timerController, ShowMainMenu);
					break;
				default:
					Debug.Log(Constants.DefaultErrorMessage);
					break;
			}

			_menuController?.Clear();
		}
	}
}
