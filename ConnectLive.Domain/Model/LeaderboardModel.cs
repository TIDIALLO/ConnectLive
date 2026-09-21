namespace ConnectLive.Domain.Model;
public class LeaderboardModel
{
    public UserModel PlayerId { get; set; }
    public int Rank { get; set; }
    public int Score { get; set; }
}
