using System;
using Configs;
using Core;

namespace Game
{
	public class GameControllerPvPNetwork : GameControllerPvP, INetwork
	{
		public GameControllerPvPNetwork(ICommonFactory factory, GameConfig config, ITimerController timerController,
			Action showMainMenu, GameMode mode)
			: base(factory, config, timerController, showMainMenu, mode)
		{
		}

		/// <summary>
		/// Lobby created and available for other players to join
		/// </summary>
		public void OnLobbyCreated()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// User joined another lobby. Here the system decides you will be X or O
		/// </summary>
		public void OnLobbyJoined()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// User left the lobby
		/// </summary>
		public void OnLobbyLeft()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The game started, user waits for his turn
		/// </summary>
		public void OnGameStarted()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The game started, both users getting back to lobby and can play again or leave
		/// </summary>
		public void OnGameFinished()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// When other user clicks a cell, the local user will receive an ID only, everything else will be done locally
		/// </summary>
		/// <param name="id">Received ID of clicked sell</param>
		public void OnCellClickReceived(byte id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// When local user clicks a cell, the other user will receive an ID only, everything else will be done locally
		/// </summary>
		/// <param name="id">ID of clicked sell to send over the Network</param>
		public void SendCellClicked(byte id)
		{
			throw new NotImplementedException();
		}
	}
}
