using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using EcAPI.Entities;
using EcAPI.Entity.OrderAggregrate;

namespace EcAPI.Interfaces
{
    

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string? basketId);
        // Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
        // Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
}