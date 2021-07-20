namespace Core
{
	public interface INetwork
	{
		void OnLobbyCreated();
		void OnLobbyJoined();
		void OnLobbyLeft();
		void OnGameStarted();
		void OnGameFinished();
		void OnCellClickReceived(byte id);
		void SendCellClicked(byte id);
	}
}
