namespace ConntectLive.DAL;
public interface IGenericRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IQueryable<TEntity>> GetAll();
    Task AddAsync(TEntity entity);
    Task Update(TEntity entity);
    Task<bool> Delete(TEntity entity);
}

#region Old way / long way
public interface IUserRepository
{
    UserEntity GetById(Guid id);
    void Add(UserEntity entity);
    void Update(UserEntity entity);
    bool Delete(UserEntity entity);
}

public interface IQuestionRepository
{
    QuestionEntity GetById(Guid id);
    void Add(UserEntity entity);
    void Update(UserEntity entity);
    bool Delete(UserEntity entity);
}

#endregion
