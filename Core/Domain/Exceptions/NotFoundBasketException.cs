using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class NotFoundBasketException : NotFoundException
    {
        public NotFoundBasketException(string Id) 
            : base($"Basket With Id {Id} Was Not Found")
        {
        }
    }
}
