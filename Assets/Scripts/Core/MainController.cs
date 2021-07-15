using Configs;
using Menu;
using UnityEngine;

namespace Core
{
	public class MainController : MonoBehaviour
	{
		[SerializeField] private GameConfig _config;
		[SerializeField] private Transform _canvasUiTransform;

		private MenuController _menuController;
		private ICommonFactory _commonFactory;

		private void Awake()
		{
			_commonFactory = new CommonFactory();

			InitControllers();
		}

		private void InitControllers()
		{
			_menuController = new MenuController(_commonFactory, _config.MainMenuPrefab, _canvasUiTransform);
		}
	}
}
