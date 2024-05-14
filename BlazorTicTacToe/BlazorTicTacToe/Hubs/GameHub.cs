using BlazorTicTacToe.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorTicTacToe.Hubs
{
	public class GameHub : Hub
	{

		private static readonly List<GameRoom> _rooms = new List<GameRoom>();

		public override async Task OnConnectedAsync()
		{
			Console.WriteLine($"Player with Id '{Context.ConnectionId}' connected");

			await Clients.Caller.SendAsync("Rooms", _rooms.OrderBy(room => room.RoomName));

		}





	}
}
