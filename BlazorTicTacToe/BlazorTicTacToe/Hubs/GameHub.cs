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


		public async Task<GameRoom> CreateRoom(string roomName, string playerName)
		{
			var roomId = Guid.NewGuid().ToString();
			var room = new GameRoom(roomId, roomName);
			_rooms.Add(room);

			var newPlayer = new Player(Context.ConnectionId, playerName);

			room.TryAddPlayer(newPlayer);


			await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

			await Clients.All.SendAsync("Rooms", _rooms.OrderBy(room => room.RoomName));

			return room;
		}

		public async Task<GameRoom?> JoinRoom(string roomId, string playerName)
		{
			var room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
			if (room is not null)
			{
				var newPlayer = new Player(Context.ConnectionId, playerName);
				if (room.TryAddPlayer(newPlayer))
				{
					await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
					await Clients.Group(roomId).SendAsync("PlayerJoined", newPlayer);
					return room;
				}
			}

			return null;
		}

		public async Task StartGame(string roomId)
		{
			var room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
			if (room is not null)
			{
				room.Game.StartGame();
				await Clients.Group(roomId).SendAsync("UpdateGame",room);
			}
		}


	}
}
