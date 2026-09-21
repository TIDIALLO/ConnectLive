using AutoMapper;
using Connectlive.Proxy;
using ConnectLive.Application;
using ConnectLive.Application.Exceptions;
using ConnectLive.Application.Interfaces;
using ConnectLive.Domain.Contracts;
using ConnectLive.Domain.Model;
using ConnectLive.Portal.Shared.Response;
using ConntectLive.DAL;
using Hangfire;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Workers;

namespace ConnectLive.Core.Api.Queries;

public static class UserQueries
{
    #region GetUser
    public class GetUserQuery : IntegrationEvent, IRequest<SaveUserResponse>, ICachable<SaveUserResponse>
    {
        public GetUserQuery(Guid userId)
        {
            UserId = userId;
            Key = userId.ToString();
        }

        public Guid UserId { get; set; }
        public string Key { get; set; }
        public int Expiration { get; set; } = 30;
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, SaveUserResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public async Task<SaveUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var persisted = await _dbContext.Users.FirstOrDefaultAsync(e => e.Id == request.UserId, cancellationToken);
            return persisted == null ? null : _mapper.Map<SaveUserResponse>(persisted);
        }
    }
    #endregion

    #region GetUsers
    public class GetUsersQuery : IntegrationEvent, IRequest<List<SaveUserResponse>>, ICachable<List<SaveUserResponse>>
    {
        public GetUsersQuery()
        {
        }

        public GetUsersQuery(Guid eventId): base(eventId)
        {
        }

        public string Key { get; set; } = "AllUsers";
        public int Expiration { get; set; } = 10;
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<SaveUserResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMapper _mapper;
        private readonly IEmailWorker _emailWorker;
        private readonly IProxy _proxy;
        private readonly IPublishEndpoint _busClient;
        private readonly IConfiguration _configuration;

        public GetUsersQueryHandler(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _busClient = serviceProvider.GetRequiredService<IPublishEndpoint>();
            _logger = serviceProvider.GetRequiredService<ILogger<GetUsersQueryHandler>>();
            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            _proxy = serviceProvider.GetRequiredService<IProxy>();
            _emailWorker = serviceProvider.GetRequiredService<IEmailWorker>();
            _backgroundJobClient = serviceProvider.GetRequiredService<IBackgroundJobClient>();
        }

        public async Task<List<SaveUserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users.ToListAsync();
            #region Deprecated
            //await _busClient.Publish<UserCreatedContractQuery>(new UserCreatedContractQuery
            //{
            //    Email = "",
            //    FullName = "",
            //});

            //var requestCommand = new RequestCommand
            //{
            //    Uri = $"{_configuration["NewletterUri"]}WeatherForecast/notification-count",
            //};

            //var content = await _proxy.Get<NotificationModel>(requestCommand);
            //_logger.LogInformation($"----- result call to {requestCommand.Uri}:{content.Data}");
            //_logger.LogWarning("Warning : {@request}", request);
            //_logger.LogError("ERROR : {@request}", request);

            ////Background Job.

            //_backgroundJobClient.Enqueue(() => _emailWorker.SendEmail("Email", "Mohammed", "Welcome to the website."));
            //_backgroundJobClient.Schedule(() => _emailWorker.SendNewsletter("NewLetter", "SALES !!!!"), TimeSpan.FromSeconds(1));
            #endregion

            return result == null ? null : _mapper.Map<List<SaveUserResponse>>(result);
        }
    }
    #endregion
}
