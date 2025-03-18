using HonoursProject.Data.Enum;
using System.Collections.Generic;

namespace HonoursProject.Models
{
    public class LobbySearchViewModel
    {
        public int? GameId { get; set; } 
        public GamePlatform? Platform { get; set; } 
        public string? Language { get; set; } 
        public List<Lobby> Lobbies { get; set; } = new List<Lobby>();
        public List<Game> Games { get; set; } = new List<Game>();
        public List<GamePlatform> Platforms { get; set; } = new List<GamePlatform>();
        public List<string> Languages { get; set; } = new List<string>();
    }
}
