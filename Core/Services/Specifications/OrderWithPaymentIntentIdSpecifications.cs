using Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderWithPaymentIntentIdSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrderWithPaymentIntentIdSpecifications(string PaymetnIntentId) : base(o=> o.PaymentIntentId == PaymetnIntentId)
        {
        }
    }
}
