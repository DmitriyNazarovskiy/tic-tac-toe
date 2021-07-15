using Core;
using UnityEngine;

namespace Menu
{
	public class MenuController
	{
		private readonly MenuModel _model;
		private MenuView _view;

		public MenuController(ICommonFactory factory, GameObject menuPrefab, Transform canvas)
		{
			_model = new MenuModel();

			InitView(factory, menuPrefab, canvas);
		}

		private void InitView(ICommonFactory factory, GameObject menuPrefab, Transform canvas)
		{
			_view = factory.InstantiateObject<MenuView>(menuPrefab, canvas);
			_view.InitStartButton(StartButtonClicked);
			_view.InitModesDropdown(_model.GetGameModesTitles());
		}

		private void StartButtonClicked(int modeId)
		{
			Debug.Log(modeId +1);
		}
	}
}
