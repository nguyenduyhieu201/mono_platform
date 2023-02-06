using Microsoft.EntityFrameworkCore.Storage;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Repository.GenericRepository.Interface
{
    public interface IRepositoryBase
    {
        IQueryable<T> GetAll<T>() where T : class;
        IQueryable<T> GetByCondition<T>(Expression<Func<T, bool>> condition) where T : class;
        Task<int> CountAsync<T>(Expression<Func<T, bool>> condition) where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> condition) where T : class;
        Task<T?> GetByIdAsync<T>(int id) where T : class;
        Task<T> CreateAsync<T>(T entity) where T : class;
        Task<IList<T>> CreateManyAsync<T>(List<T> entities) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task DeleteAsync<T>(T entity) where T : class;
        Task DeleteManyAsync<T>(List<T> entities) where T : class;
        IDbContextTransaction BeginTransaction();
    }
}
