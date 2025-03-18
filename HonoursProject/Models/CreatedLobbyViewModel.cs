public class CreatedLobbyViewModel
{
    public int LobbyId { get; set; }
    public string GameTitle { get; set; }
    public string Platform { get; set; }
    public List<UserInfo> Participants { get; set; } = new();
    public string ChatMessages { get; set; } = string.Empty;
}

public class UserInfo
{
    public string UserName { get; set; }
    public string GamingId { get; set; }
    public string ProfilePictureUrl { get; set; }
}
