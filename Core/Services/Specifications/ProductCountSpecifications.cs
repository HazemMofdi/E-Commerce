using Domain.Entities.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductCountSpecifications(ProductSpecificationParameters parameters)
            : base(p => (!parameters.TypeId.HasValue || parameters.TypeId == p.TypeId) &&
                        (!parameters.BrandId.HasValue || parameters.BrandId == p.BrandId) &&
                        (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search)))
        {

        }
    }
}
