using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HonoursProject.Models
{
    public class UserRegister
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        // Make optional fields nullable
        public string? DiscordId { get; set; }
        public string? XboxId { get; set; }
        public string? PS5Id { get; set; }
        public string? SteamId { get; set; }

        public IFormFile? ProfileImage { get; set; } // Make nullable for optional file upload
    }
}


