using System.Linq;
using Configs;
using Game;
using Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class MainController : MonoBehaviour
	{
		[SerializeField] private Image _background;
		[SerializeField] private GameConfig _config;
		[SerializeField] private Transform _canvasTransform;
		[SerializeField] private Camera _camera;

		private ICommonFactory _commonFactory;
		private MenuController _menuController;
		private GameControllerBase _gameController;
		private ITimerController _timerController;
		private bool _isArtUpdated;

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
			_menuController.CreateView(_commonFactory, _config.MainMenuPrefab, _canvasTransform, StartGame, UpdateArt);

			if (_isArtUpdated)
				_menuController.HideUpdateArtButton();

			_gameController?.Clear();
			_gameController = null;
		}

		private void UpdateArt()
		{
			if (_isArtUpdated)
				return;

			var bundle = AssetBundle.LoadFromFile(PlayerPrefs.GetString(Constants.PlayerPrefsBundleKey));
			var assets = bundle.LoadAllAssets<Sprite>();

			_config.LoadedX = assets.First(a => a.name == _config.X.name);
			_config.LoadedO = assets.First(a => a.name == _config.O.name);

			_background.sprite = assets.First(a => a.name == _background.sprite.name);

			_menuController.HideUpdateArtButton();

			_isArtUpdated = true;
		}

		private void Update()
		{
			if (_gameController == null)
				return;

			var deltaTime = Time.deltaTime;

			_gameController?.Update(deltaTime);
		}

		private void StartGame(int modeId)
		{
			var mode = (GameMode) modeId;

			switch (mode)
			{
				case GameMode.PlayerVsPlayer:
					_gameController = new GameControllerPvP(_commonFactory, _config, _timerController, ShowMainMenu, mode);
					break;
				case GameMode.PlayerVsPc:
					_gameController = new GameControllerPvPC(_commonFactory, _config, _timerController, ShowMainMenu, mode);
					break;
				default:
					Debug.Log(Constants.DefaultErrorMessage);
					break;
			}

			_menuController?.Clear();
			_gameController?.CreateGameView(_canvasTransform, _config.GameViewPrefab);
		}
	}
}
