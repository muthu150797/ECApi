using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entities;
using EcAPI.Entity.OrderAggregrate;

namespace EcAPI.Interfaces
{
    public interface IOrderRepository
    {
        Task<DeliveryMethod> CreateOrder(int id);
        Task<Order> GetOrderByPaymentId(string id);
        dynamic SaveOrderToDb(Order order);
        int DeleteOrder(Order order);
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        Task<DeliveryMethod> GetDeliveryMethodByIdAync(int? id);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}