using System.Linq.Expressions;
using ServiceWithBlazor.Models;

namespace ServiceWithBlazor.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);


    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByEmailAsync(string email);
}

