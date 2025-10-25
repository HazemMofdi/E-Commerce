using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.DTOs.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class BasketController(IServiceManager serviceManager) : ApiController
    {
        //Get
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasketAsync(string Id)
        {
            return Ok(await serviceManager.BasketService.GetBasketAsync(Id));
        }
        //Post
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasketAsync(BasketDTO basketDTO)
        {
            return Ok(await serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDTO));
        }

        //Delete
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteBasket(string Id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(Id);
            return NoContent();
        }

    }
}
