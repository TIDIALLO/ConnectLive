using ConntectLive.DAL;

namespace ConnectLive.Core.Api.Decorators;

public interface IArticleUnitOfWork<C> : IUnitOfWork<C>
{
    public IGenericRepository<UserEntity> Users { get; set; }
}
