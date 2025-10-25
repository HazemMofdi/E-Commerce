using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared;
using Shared.DTOs.ProductModule;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync()
        {
            var Brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandsResult = mapper.Map<IEnumerable<BrandResultDTO>>(Brands);
            return brandsResult;
        }
        public async Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync()
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<TypeResultDTO>>(Types);
            
        }
        public async Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpecificationParameters parameters)
        {
            var productRepo = unitOfWork.GetRepository<Product, int>();

            var specifications = new ProductWithBrandAndTypeSpecifications(parameters);
            var Products = await productRepo.GetAllAsync(specifications);
            var ProductsResultDTO = mapper.Map<IEnumerable<ProductResultDTO>>(Products);
            var pageSize = Products.Count();
            var countSpecifications = new ProductCountSpecifications(parameters);
            var totalCount = await productRepo.CountAsync(countSpecifications);
            return new PaginatedResult<ProductResultDTO>(parameters.PageIndex, pageSize, totalCount, ProductsResultDTO);
        }
        public async Task<ProductResultDTO> GetProductByIdAsync(int Id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(Id);
            var Product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(specifications);
            //return mapper.Map<ProductResultDTO>(Product);
            return Product is null? throw new ProductNotFoundException(Id): mapper.Map<ProductResultDTO>(Product);
        }
    }
}
