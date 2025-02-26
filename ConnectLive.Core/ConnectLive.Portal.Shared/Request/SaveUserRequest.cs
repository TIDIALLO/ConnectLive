using System.ComponentModel.DataAnnotations;

namespace ConnectLive.Portal.Shared.Request;

public class SaveUserRequest
{
    public string UserFirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }
}

