using HonoursProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HonoursProject.Models
{
    public class GamePlatformMapping
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }
        public Game Game { get; set; }

        public GamePlatform Platform { get; set; } // Enum value
    }
}

