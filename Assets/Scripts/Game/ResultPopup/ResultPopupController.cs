using System;
using Core;
using UnityEngine;

namespace Game.ResultPopup
{
	public class ResultPopupController : IClearable
	{
		private readonly ResultPopupModel _model;
		private ResultPopupView _view;

		public ResultPopupController() => _model = new ResultPopupModel();

		public void CreateView(ICommonFactory factory, GameObject prefab, Transform parent)
			=> _view = factory.InstantiateObject<ResultPopupView>(prefab, parent);
		public void InitButtons(Action restartButtonAction, Action menuButtonAction)
			=> _view.InitButtons(restartButtonAction, menuButtonAction);
		public void SetResultMessage(GameResult result) => _view.SetResult(_model.GetPopupMessage(result));

		public void Clear()
		{
			_view.Clear();
			_view = null;
		}
	}
}
