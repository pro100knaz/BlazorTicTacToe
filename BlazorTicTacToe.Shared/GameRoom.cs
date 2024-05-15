using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTicTacToe.Shared
{
	public class GameRoom
	{
		public string RoomId { get; set; }
		public string RoomName { get; set; }

		public List<Player> Players { get; set; } = new();

		public TicTacToeGame Game { get; set; } = new();

		public GameRoom(string roomId, string roomName)
		{
			RoomName = roomName;
			RoomId = roomId;
		}

		public bool TryAddPlayer(Player newPlayer)
		{
			if (Players.Count < 2 && Players.All(p => p.ConnectionId != newPlayer.ConnectionId))
			{
				Players.Add(newPlayer);
				if (Players.Count == 1)
				{
					Game.PlayerXId = newPlayer.ConnectionId;
				}
				else if(Players.Count == 2)
				{
					Game.PlayerOId = newPlayer.ConnectionId;
				}
				return true;
			}

			return false;
		}
	}
}
