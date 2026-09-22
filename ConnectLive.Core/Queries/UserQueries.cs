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
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public async Task<List<SaveUserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users.ToListAsync();

            // Publishing UserCreatedContractQuery, calling the newsletter count via IProxy, and enqueuing
            // Hangfire email/newsletter jobs were only ever demonstrated here, commented out, and required
            // MassTransit/Hangfire services this handler no longer eagerly resolves. Re-add the relevant
            // constructor dependency if one of these is wired up for real.

            return result == null ? null : _mapper.Map<List<SaveUserResponse>>(result);
        }
    }
    #endregion
}
