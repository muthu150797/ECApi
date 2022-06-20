using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcAPI.Repository;
using EcAPI.Interfaces;
using EcAPI.Entities;

namespace EcAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        public readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        [HttpGet]
        [Route("GetBasketById")]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket);
        }
        [HttpPost]
        [Route("UpdateBasket")]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket)
        {
            var baskets = await _basketRepository.UpdateBasketAsync(basket);
            return Ok(baskets);
        }
        [HttpDelete]
        [Route("DeleteBasketById")]
        public async Task DeleteBasketByIdAsync(string id)
        {
             await _basketRepository.DeletebasketAsync(id);
            ///return Ok(baskets);
        }
    }
}