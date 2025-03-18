using HonoursProject.Data.Enum;
using HonoursProject.Models;
using System.ComponentModel.DataAnnotations;

namespace HonoursProject.ViewModels
{
    public class CreateLobbyViewModel
    {
        [Required(ErrorMessage = "Please select a game.")]
        public int GameId { get; set; }

        [Required(ErrorMessage = "Please select a platform.")]
        public GamePlatform GamePlatform { get; set; }

        [Required(ErrorMessage = "Please enter the maximum number of players.")]
        [Range(4, 12, ErrorMessage = "Max players must be between 4 and 12.")]
        public int MaxPlayers { get; set; }

        [Required(ErrorMessage = "Please select a preferred language.")]
        public string PreferredLanguage { get; set; }

        [Required(ErrorMessage = "Please select if a mic is required.")]
        public bool MicRequired { get; set; }

        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; }

       
        public List<Game> Games { get; set; } = new List<Game>();
        public List<string> Languages { get; set; } = new List<string>();
    }
}

