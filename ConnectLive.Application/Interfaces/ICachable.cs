namespace ConnectLive.Application.Interfaces;
public interface ICachable<TEntity>
{
    /// <summary>
    /// Set or Get the cache Key.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Set the cache expiration in seconds.
    /// </summary>
    public int Expiration { get; set; }
}
