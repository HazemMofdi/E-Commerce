using Domain.Entities;
using Domain.Entities.ProductModule;
using Shared;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandAndTypeSpecifications(ProductSpecificationParameters parameters) 
            : base(p=> (!parameters.TypeId.HasValue || parameters.TypeId == p.TypeId) && 
                       (!parameters.BrandId.HasValue || parameters.BrandId == p.BrandId)&&
                       (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search)))
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
            //
            switch(parameters.Sort)
            {
                case ProducsSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProducsSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProducsSortingOptions.PriceAsc: 
                    AddOrderBy(p => p.Price);
                    break;
                case ProducsSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price); 
                    break;
                default:
                    break;
            }

            ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }
        public ProductWithBrandAndTypeSpecifications(int Id) : base(p=> p.Id == Id)
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
        }
    }
}
