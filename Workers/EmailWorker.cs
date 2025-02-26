using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Workers;

public class EmailWorker(ILogger<EmailWorker> logger) : IEmailWorker
{
    public ILogger<EmailWorker> Logger { get; } = logger;

    [DisplayName("{0}")]
    [Hangfire.Queue("email")]
    public void SendEmail(string jobName, string name, string message)
    {
        Logger.LogInformation($"[{Assembly.GetExecutingAssembly().FullName}] - ======>>>>>>> EMAIL: {name}, {message}");
    }

    [DisplayName("{0}")]
    [Hangfire.Queue("email")]
    public void SendEmail2(string jobName, string name, string message)
    {
        Logger.LogInformation($"[{Assembly.GetExecutingAssembly().FullName}] - ======>>>>>>> EMAIL: {name}, {message}");
    }

    [DisplayName("{0}")]
    [Hangfire.Queue("newsletter")]
    public void SendNewsletter(string jobName, string message)
    {
        Logger.LogInformation($"[{Assembly.GetExecutingAssembly().FullName}] - $$$$$ NEWSLETTER: {message}");
    }
}


public interface IEmailWorker
{
    void SendEmail(string jobName, string name, string message);
    void SendEmail2(string jobName, string name, string message);
    void SendNewsletter(string jobName, string message);
}