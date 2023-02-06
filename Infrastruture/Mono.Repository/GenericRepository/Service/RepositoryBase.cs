using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mono.Repository.Data;
using Mono.Repository.GenericRepository.Interface;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Repository.GenericRepository.Service
{
    public abstract class RepositoryBase : IRepositoryBase
    {
        protected readonly RepositoryContext _dbContext;

        public RepositoryBase(RepositoryContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<T> GetAll<T>() where T : class
            => _dbContext.Set<T>().AsNoTracking();

        public IQueryable<T> GetByCondition<T>(Expression<Func<T, bool>> condition) where T : class
            => _dbContext.Set<T>().AsNoTracking().Where(condition);

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> condition) where T : class
            => await _dbContext.Set<T>().AsNoTracking().CountAsync(condition);

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> condition) where T : class
            => await _dbContext.Set<T>().AsNoTracking().AnyAsync(condition);

        public async Task<T?> GetByIdAsync<T>(int id) where T : class
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<T> CreateAsync<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IList<T>> CreateManyAsync<T>(List<T> entities) where T : class
        {
            _dbContext.Set<T>().AddRange(entities);
            await _dbContext.SaveChangesAsync();
            return entities;
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteManyAsync<T>(List<T> entities) where T : class
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction() => _dbContext.Database.BeginTransaction();
    }
}
