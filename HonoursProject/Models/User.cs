using System.ComponentModel.DataAnnotations;

namespace HonoursProject.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? DiscordId { get; set; }      // Optional
        public string? XboxId { get; set; }         // Optional
        public string? PS5Id { get; set; }          // Optional
        public string? SteamId { get; set; }        // Optional
        public string? ProfileImageURL { get; set; } // Optional

        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; } // To be able to ban people
        public ICollection<LobbyParticipant> LobbiesJoined { get; set; }

    }
}

