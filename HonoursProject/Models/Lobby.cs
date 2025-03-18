using HonoursProject.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HonoursProject.Models
{
    public class Lobby
    {
        [Key]
        public int LobbyId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Creator { get; set; }

        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; }

        public int MaxPlayers { get; set; }

        public GamePlatform GamePlatform { get; set; }

        public bool? MicRequired { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public string PreferredLanguage { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        // ✅ Updated to use the join table
        public ICollection<LobbyParticipant> Participants { get; set; }
    }
}
