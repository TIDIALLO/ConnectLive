namespace Connectlive.Proxy;
public class RequestCommand
{
    public string? Uri { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new ();
    public Dictionary<string, object> Body { get; set; } = new ();

}
