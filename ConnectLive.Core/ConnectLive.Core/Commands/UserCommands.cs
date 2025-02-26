using AutoMapper;
using ConnectLive.Application;
using ConnectLive.Portal.Shared.Request;
using ConnectLive.Portal.Shared.Response;
using ConntectLive.DAL;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using Workers;

namespace ConnectLive.Core.Api.Commands;

public static class UserCommands
{
    #region SaveUser
    public class SaveUserCommand : IntegrationEvent, IRequest<SaveUserResponse>
    {
        public SaveUserCommand(SaveUserRequest user)
        {
            User = user;
        }

        public SaveUserRequest User { get; set; }
    }

    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, SaveUserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SaveUserCommandHandler(IServiceProvider serviceProvider)
        {
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<SaveUserResponse> Handle(SaveUserCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<UserEntity>(command.User);
            await _unitOfWork.Users.AddAsync(entity);
            _unitOfWork.Commit();
            var persisted = await _unitOfWork.Users.GetByIdAsync(entity.Id);
            return _mapper.Map<SaveUserResponse>(persisted);
        }
    }
    #endregion
}
