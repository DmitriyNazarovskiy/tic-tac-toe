using System;
using Core;
using UnityEngine;

namespace Menu
{
	public class MenuController : IClearable
	{
		private readonly MenuModel _model;

		private MenuView _view;

		public MenuController() => _model = new MenuModel();

		public void CreateView(ICommonFactory factory, GameObject menuPrefab, Transform canvas, Action<int> startGameClicked,
			Action updateArt)
		{
			_view = factory.InstantiateObject<MenuView>(menuPrefab, canvas);
			_view.InitButtons(startGameClicked, updateArt);
			_view.InitModesDropdown(_model.GetGameModesTitles());
		}

		public void HideUpdateArtButton() => _view.HideUpdateArtButton();
		public void Clear() => _view.Clear();
	}
}
