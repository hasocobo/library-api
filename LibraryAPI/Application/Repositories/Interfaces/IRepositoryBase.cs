using System.Linq.Expressions;

namespace LibraryAPI.Application.Repositories.Interfaces;

public interface IRepositoryBase<T> 
{
    public IQueryable<T> FindAll();
    
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate);
    
    public void Create(T entity);
    public void Update(T entity);
    public void Delete(T entity);
    
    public Task<bool> Exists(Expression<Func<T, bool>> predicate);
}