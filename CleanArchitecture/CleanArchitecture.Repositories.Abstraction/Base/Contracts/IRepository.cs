using CleanArchitecture.Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Repositories.Abstraction.Base.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // Queries
        Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetFirstOrDefaultAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByIdAsync(long id);
        
        // Commands
        Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task<Result<IList<TEntity>>> AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken);
        Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<Result<IList<TEntity>>> UpdateRangeAsync(IList<TEntity> entity, CancellationToken cancellationToken);
        Task<Result<TEntity>> DeleteAsync(TEntity entity);
        Task<Result<TEntity>> DeleteAsync(long id);
        Task<Result<IList<TEntity>>> DeleteRangeAsync(IList<TEntity> entity);
    }
}
