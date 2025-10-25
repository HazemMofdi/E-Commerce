﻿using Domain.Contracts;
using Domain.Entities;
using Presistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(AppDbContext dbContext) :
        IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
        {
           return asNoTracking ? await dbContext.Set<TEntity>().AsNoTracking().ToListAsync() : 
                await dbContext.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(TKey Id)
        {
            return await dbContext.Set<TEntity>().FindAsync(Id);
        }

        public async Task AddAsync(TEntity entity)
        {
             await dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }
        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }

        #region Specifications
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), specifications).ToListAsync();

        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), specifications).CountAsync();
        }
        #endregion
    }
}
