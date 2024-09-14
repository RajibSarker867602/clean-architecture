using CleanArchitecture.Domain.Entities.Base;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Repositories.Abstraction.Base.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Repositories.Abstraction.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _db;

        protected Repository(DbContext db)
        {
            _db = db;
        }

        private DbSet<TEntity> Table
        {
            get
            {
                return _db.Set<TEntity>();
            }
        }

        public async Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null) return Result.Failure<TEntity>(new Error("Failed", "No data found to add!"));

            await Table.AddAsync(entity, cancellationToken);

            return await _db.SaveChangesAsync() > 0 ? Result.Success<TEntity>(entity) :
                Result.Failure<TEntity>(new Error("Failed", "Data could not be added!"));
        }

        public async Task<Result<IList<TEntity>>> AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities is null) return Result.Failure<IList<TEntity>>(new Error("Failed", "No data found to add!"));

            await Table.AddRangeAsync(entities, cancellationToken);

            return await _db.SaveChangesAsync() > 0 ? Result.Success<IList<TEntity>>(entities) :
                Result.Failure<IList<TEntity>>(new Error("Failed", "Data could not be added!"));
        }

        public async Task<Result<TEntity>> DeleteAsync(long id)
        {
            if (id <= 0) return Result.Failure<TEntity>(new Error("Failed", "No data found to delete!"));

            var dataToRemove = await GetByIdAsync(id);
            if(dataToRemove is null) Result.Failure<TEntity>(new Error("Not Found", $"Data not found by this id of {id}"));

            Table.Remove(dataToRemove);

            return await _db.SaveChangesAsync() > 0 ? Result.Success<TEntity>(dataToRemove) :
                Result.Failure<TEntity>(new Error("Failed", "Data could not be deleted!"));
        }

        public async Task<Result<TEntity>> DeleteAsync(TEntity entity)
        {
            if (entity is null) return Result.Failure<TEntity>(new Error("Failed", "No data found to delete!"));

            Table.Remove(entity);

            return await _db.SaveChangesAsync() > 0 ? Result.Success<TEntity>(entity) :
                Result.Failure<TEntity>(new Error("Failed", "Data could not be deleted!"));
        }

        public async Task<Result<IList<TEntity>>> DeleteRangeAsync(IList<TEntity> entity)
        {
            if (entity is null) return Result.Failure<IList<TEntity>>(new Error("Failed", "No data found to delete!"));

            Table.RemoveRange(entity);

            return await _db.SaveChangesAsync() > 0 ? Result.Success<IList<TEntity>>(entity) :
                Result.Failure<IList<TEntity>>(new Error("Failed", "Data could not be deleted!"));
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity,
            bool>> predicate) => Table.Where(predicate);

        public async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            => await Table.ToListAsync(cancellationToken);

        public async Task<TEntity> GetByIdAsync(long id) => await Table.FindAsync(id);

        public async Task<TEntity> GetFirstOrDefaultAsNoTrackingAsync(Expression<Func<TEntity,
            bool>> predicate) => await Table.AsNoTracking().FirstOrDefaultAsync(predicate);

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity,
            bool>> predicate) => await Table.FirstOrDefaultAsync(predicate);

        public Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IList<TEntity>>> UpdateRangeAsync(IList<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IList<TEntity>>> UpdateRangeAsync(IList<TEntity> entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
