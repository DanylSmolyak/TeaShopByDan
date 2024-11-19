using System.Linq.Expressions;

namespace Core.Interfaces;

public interface IRepository<T>  
    where T : class
{
    Task<T> GetbyIDAsync(int id);
    Task<T> GetbyIDAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> ListAsync();
    Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task DeleteAsync(T entity );
    Task EditAsync(T entity);   
    IQueryable<T> FindWithSpecification(ISpecification<T> specification = null);
    Task<int> CountAsync(ISpecification<T> specification = null); 
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task EditRangeAsync(IEnumerable<T> entity);


}