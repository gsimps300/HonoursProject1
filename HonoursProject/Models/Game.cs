using HonoursProject.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace HonoursProject.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        // Many-to-many relationship with GameGenre
        public List<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

        // ✅ Initialize Platforms to avoid null reference errors
        public List<GamePlatform> Platforms { get; set; } = new List<GamePlatform>();

        public bool IsCrossPlatform { get; set; }
        public string ImageURL { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public bool ActiveStatus { get; set; }

        public List<GamePlatformMapping> PlatformMappings { get; set; } = new List<GamePlatformMapping>();

    }
}

