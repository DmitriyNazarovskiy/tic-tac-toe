namespace Core
{
	public enum GameMode : byte
	{
		None = 0,
		PlayerVsPlayer,
		PlayerVsPc,
		Network
	}

	public enum CellState : byte
	{
		Clear = 0,
		X,
		O
	}

	public enum GameResult : byte
	{
		None,
		Player1Win,
		Player2Win,
		Draw,
		TimeOver
	}

	public enum PlayerType : byte
	{
		None = 0,
		Player1,
		Player2,
		PC
	}
}
