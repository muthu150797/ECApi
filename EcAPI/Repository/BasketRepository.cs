using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EcAPI.Entities;
using EcAPI.Interfaces;
using StackExchange.Redis;

namespace EcAPI.Repository
{
    public class BasketRepository : IBasketRepository
    {
        public readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeletebasketAsync(string basketId)
        {
           return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
             var created=await _database.StringSetAsync(basket.Id,
             JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));
             if(!created) return null;
             return await GetBasketAsync(basket.Id);
        }
    }
}