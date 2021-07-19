using Core;

namespace Game.Cell
{
	public class CellModel
	{
		public byte Id { get; set; }
		public CellState CurrentState { get; set; }

		public CellModel(byte id) => Id = id;
	}
}
