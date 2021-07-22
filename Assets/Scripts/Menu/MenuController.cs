using System;
using Configs;
using Core;
using UnityEngine;

namespace Menu
{
	public class MenuController : IClearable
	{
		private readonly MenuModel _model;

		private MenuView _view;

		public MenuController(GameConfig config) => _model = new MenuModel(config);

		public void HideUpdateArtButton() => _view.HideUpdateArtButton();

		public void CreateView(ICommonFactory factory, GameObject menuPrefab, Transform canvas, Action<int> startGameClicked,
			Action updateArt)
		{
			_view = factory.InstantiateObject<MenuView>(menuPrefab, canvas);
			_view.InitButtons(startGameClicked, updateArt);
			_view.InitModesDropdown(_model.GetGameModesTitles(), OnGameModeValueChanged);
		}

		private void OnGameModeValueChanged(int value)
		{
			AudioPlayer.PlayEffect(_model.Config.AudioConfig.TapSound, Constants.DefaultSoundVolume);

			if ((GameMode) value + 1 != GameMode.Network) //+1 because we need to skip "None" mode which is 0
			{
				_view.SetStartButtonState(true, MenuModel.StartLabel);

				return;
			}

			_view.SetStartButtonState(false, MenuModel.SoonLabel);
		}
		
		public void Clear() => _view.Clear();
	}
}
