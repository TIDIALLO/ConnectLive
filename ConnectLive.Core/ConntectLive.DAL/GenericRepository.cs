using Microsoft.EntityFrameworkCore;

namespace ConntectLive.DAL;
public class GenericRepository<TEntity>(ApplicationDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task<bool> Delete(TEntity entity)
    {
        await Task.CompletedTask;
        if (_dbContext.Entry(entity).State == EntityState.Detached) _dbContext.Attach(entity);
        _dbContext.Set<TEntity>().Remove(entity);
        return true;
    }

    public async Task<IQueryable<TEntity>> GetAll()
    {
        await Task.CompletedTask;
        return _dbContext.Set<TEntity>().AsNoTracking();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        await Task.CompletedTask;
        return _dbContext.Set<TEntity>().FirstOrDefault(x => x.Id == id);
    }

    public async Task Update(TEntity entity)
    {
        await Task.CompletedTask;
        _dbContext.Set<TEntity>().Update(entity);
    }
}

#region Old way / long way
public class UserRepository : IUserRepository
{
    public void Add(UserEntity entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(UserEntity entity)
    {
        throw new NotImplementedException();
    }

    public UserEntity GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(UserEntity entity)
    {
        throw new NotImplementedException();
    }
}

public class QuestionRepository : IQuestionRepository
{
    public void Add(UserEntity entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(UserEntity entity)
    {
        throw new NotImplementedException();
    }

    public QuestionEntity GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(UserEntity entity)
    {
        throw new NotImplementedException();
    }
}

#endregion
