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

		public void CreateView(ICommonFactory factory, GameObject menuPrefab, Transform canvas, Action<int> startGameClicked)
		{
			_view = factory.InstantiateObject<MenuView>(menuPrefab, canvas);
			_view.InitStartButton(startGameClicked);
			_view.InitModesDropdown(_model.GetGameModesTitles());
		}

		public void Clear() => _view.Clear();
	}
}
