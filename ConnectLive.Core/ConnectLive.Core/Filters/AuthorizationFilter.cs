using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace ConnectLive.Core.Api.Filters;

public class AuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}
