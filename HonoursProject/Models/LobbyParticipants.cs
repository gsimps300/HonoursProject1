namespace HonoursProject.Models
{
    public class LobbyParticipant
    {
        public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
