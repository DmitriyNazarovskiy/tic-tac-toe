using System.Collections.Generic;
using System.Linq;
using Configs;
using Core;
using Game.Cell;
using UnityEngine;

namespace Game
{
	public class GameModel
	{
		private readonly GameConfig _config;

		public bool GameInProgress { get; set; } = false;
		public CellState CurrentTurnState { get; set; } = CellState.X;
		public List<CellModel> MarkedCells { get; }

		public GameModel(GameConfig config)
		{
			_config = config;

			MarkedCells = new List<CellModel>(Constants.CellsAmount);
		}

		public Sprite GetCellSprite(CellState state) => state == CellState.X ? _config.X : _config.O;

		public GameResult CheckGameResult()
		{
			if (MarkedCells.Count == 0)
				return GameResult.None;

			var markedCellsByCurrentPlayer = MarkedCells.FindAll(mc => mc.CurrentState == CurrentTurnState);

			if (markedCellsByCurrentPlayer.Count < Constants.RequiredMarksForWin)
				return GameResult.None;

			var ids = markedCellsByCurrentPlayer.Select(cm => cm.Id).ToList();

			for (var i = 0; i < markedCellsByCurrentPlayer.Count; i++)
			{
				for (var j = 0; j < _config.WinCombinations.Length; j++)
				{
					var values =
						new [] {_config.WinCombinations[j].Value1, _config.WinCombinations[j].Value2, _config.WinCombinations[j].Value3};
					var dif = ids.Intersect(values).ToList();

					if (dif.Count == Constants.RequiredMarksForWin)
						return CurrentTurnState == CellState.X ? GameResult.Player1Win : GameResult.Player2Win;
				}
			}

			if (MarkedCells.Count == Constants.CellsAmount)
				return GameResult.Draw;

			return GameResult.None;
		}

		public string GetCurrentPlayerLabel()
		{
			var player = CurrentTurnState == CellState.X ? PlayerType.Player1 : PlayerType.Player2;

			switch (player)
			{
				case PlayerType.Player1:
					return Constants.Player1String;
				case PlayerType.Player2:
					return Constants.Player2String;
				default:
					Debug.Log(Constants.DefaultErrorMessage);
					break;
			}

			return string.Empty;
		}
	}
}
