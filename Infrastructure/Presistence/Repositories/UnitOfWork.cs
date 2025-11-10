using Domain.Contracts;
using Domain.Entities;
using Presistence.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;
        private readonly ConcurrentDictionary<string, object> repositories;
        //private readonly Dictionary<string, object> repositories;

        public UnitOfWork(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            repositories = new();
        }

        //public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        //{
        //    var key = typeof(TEntity).Name;
        //    if (!repositories.ContainsKey(key))
        //    {
        //        repositories[key] = new GenericRepository<TEntity, TKey>(dbContext);
        //    }
        //    return (IGenericRepository<TEntity, TKey>)repositories[key];
        //}

        //This implementation is faster than the ordinary DICTIONARY implementation //
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            //                                                         Very important to use the discard to match the method params
            return (IGenericRepository<TEntity, TKey>)repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity, TKey>(dbContext));
        }


        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
