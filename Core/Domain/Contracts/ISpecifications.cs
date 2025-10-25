using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // signature for property [Expression ==> Where]
        public Expression<Func<TEntity, bool>>? Criteria{ get;}
        
        // signature for property [Expression ==> Include]
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get;}

        // signature for property [Expression ==> OrderBy, OrderByDecsending]
        public Expression<Func<TEntity, object>>? OrderBy { get; }
        public Expression<Func<TEntity, object>>? OrderByDecsending { get; }

        // Pagination
        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get; }
    }
}
