using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared;
using Shared.DTOs.ProductModule;
using Shared.Enums;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    
    public class ProductsController(IServiceManager serviceManager) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDTO>>> GetAllProductsAsync([FromQuery]ProductSpecificationParameters parameters)
            => Ok(await serviceManager.ProductService.GetAllProductsAsync(parameters));

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetAllBrandsAsync()
            => Ok(await serviceManager.ProductService.GetAllBrandsAsync());

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDTO>>> GetAllTypesAsync()
            => Ok(await serviceManager.ProductService.GetAllTypesAsync());

        [ProducesResponseType(typeof(ProductResultDTO), StatusCodes.Status200OK)]
        
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ProductResultDTO>> GetProductByIdAsync(int Id)
            => Ok(await serviceManager.ProductService.GetProductByIdAsync(Id));
    }
}
