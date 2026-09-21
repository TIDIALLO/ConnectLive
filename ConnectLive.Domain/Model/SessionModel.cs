namespace ConnectLive.Domain.Model;
public class SessionModel
{
    public QuestionModel CurrentQuestion { get; set; }
    public List<UserModel> ConnectedPlayers { get; set; } = new List<UserModel>();
}