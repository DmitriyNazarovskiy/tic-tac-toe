using Core;

namespace Game.ResultPopup
{
	public class ResultPopupModel
	{
		private const string Player1Win = "Player 1 won!";
		private const string Player2Win = "Player 2 won!";
		private const string PCWin = "PC won!";
		private const string Draw = "Draw!";
		private const string TimeOver = "The time is over.";

		public string GetPopupMessage(GameResult result, GameMode mode)
		{
			switch (result)
			{
				case GameResult.None:
					break;
				case GameResult.Player1Win:
					return Player1Win;
				case GameResult.Player2Win:
					return mode == GameMode.PlayerVsPc ? PCWin : Player2Win;
				case GameResult.Draw:
					return Draw;
				case GameResult.TimeOver:
					return TimeOver;
			}

			return string.Empty;
		}
	}
}
