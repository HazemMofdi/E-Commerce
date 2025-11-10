using Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderWithIncludeSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrderWithIncludeSpecifications(Guid Id) : base(o => o.Id == Id)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.OrderItems);
            AddOrderBy(o => o.OrderDate);
        }

        public OrderWithIncludeSpecifications(string userEmail) : base(o => o.UserEmail == userEmail)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.OrderItems);
            AddOrderBy(o => o.OrderDate);
        }
    }
}
