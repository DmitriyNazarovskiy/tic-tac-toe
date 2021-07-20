using System;
using Core;
using UnityEngine;

namespace Game.Cell
{
	public class CellController
	{
		private readonly CellView _view;
		private readonly Action<byte> _cellClickAction;

		public CellModel Model { get; }

		public CellController(CellView view, Action<byte> cellClickAction = null)
		{
			_view = view;
			_view.Init(OnCellClicked);

			_cellClickAction = cellClickAction;

			Model = new CellModel(_view.GetId());
		}

		private void OnCellClicked() => _cellClickAction?.Invoke(Model.Id);
		public void SetHint(bool isShow, Material hintMaterial = null) => _view.SetHint(isShow, hintMaterial);

		public void SetCellState(CellState newState, Sprite sprite)
		{
			if(Model.CurrentState != CellState.Clear)
				return;

			Model.CurrentState = newState;

			_view.SetSprite(sprite);
		}

		public byte GetCellId() => Model.Id;
	}
}
