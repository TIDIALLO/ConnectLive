namespace ConnectLive.Portal.Shared.Response;
public class SaveUserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }

    public Token Signature { get; set; }
}

public class Token
{
    public DateTime Time { get; set; }
    public string Secret { get; set; }
}
