using System.Linq.Expressions;
using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Application.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly LibraryContext _libraryContext;

    public RepositoryBase(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public void Create(T entity)
    {
        _libraryContext.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _libraryContext.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _libraryContext.Set<T>().Update(entity);
    }

    public IQueryable<T> FindAll()
    {
        return _libraryContext.Set<T>();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _libraryContext.Set<T>().Where(expression);
    }

    public async Task<bool> Exists(Expression<Func<T, bool>> expression)
    {
        return await _libraryContext.Set<T>().AnyAsync(expression);
    }
}