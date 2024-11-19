    using System.Linq.Expressions;
    using Core.Entities;
    using Core.Interfaces;
    using Microsoft.EntityFrameworkCore;

    namespace Infrastructure.Data
    {
        public class Repository<T> : IRepository<T>
            where T : BaseEntitie
        {
            private readonly Context _context;

            public Repository(Context context)
            {
                _context = context;
            }

            public async Task<T> GetbyIDAsync(int id)
            {
                return await _context.Set<T>()
                    .Where(x => x.id == id && !x.IsDeleted)
                    .FirstOrDefaultAsync();
            }
            
            public async Task<T> GetbyIDAsync(Expression<Func<T, bool>> predicate)
            {
                return await _context.Set<T>().FirstOrDefaultAsync(predicate);
            }

            public async Task<List<T>> ListAsync()
            {
                return await _context.Set<T>().ToListAsync();
            }

            public async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate)
            {
                return await _context.Set<T>()
                    .Where(predicate)
                    .ToListAsync();
            }
                

            public async Task AddAsync(T entity)
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            public async Task EditAsync(T entity)
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
            }
            
            public async Task EditRangeAsync(IEnumerable<T> entity)
            {
                _context.Set<T>().UpdateRange(entity);
                await _context.SaveChangesAsync();
            }

            public async Task<int> CountAsync(ISpecification<T> specification = null)
            {
                return await _context.Set<T>().CountAsync();
            }

            public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
            {
                return await _context.Set<T>().CountAsync(predicate);
            }

            public async Task DeleteAsync(T entity)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            
            public IQueryable<T> FindWithSpecification(ISpecification<T> specification = null)
            {
                return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
            }
        }
    }
