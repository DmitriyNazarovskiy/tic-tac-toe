using System;

namespace Game.Cell
{
	public class CellController
	{
		private CellModel _model;
		private CellView _view;
		private Action<byte> _cellClickAction;

		public CellController(CellView view, Action<byte> cellClickAction)
		{
			_view = view;
			_view.Init(OnCellClicked);

			_cellClickAction = cellClickAction;

			_model = new CellModel(_view.GetId());
		}

		private void OnCellClicked()
		{
			_cellClickAction?.Invoke(_model.Id);
		}
	}
}
