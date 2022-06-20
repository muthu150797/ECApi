using System;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entity.OrderAggregrate;
using EcAPI.Interfaces;
using EcAPI.Interfaces.Core.Interfaces;
using EcAPI.Repository;

namespace EcAPI.Services
{
    public class OrderService : IOrderService
    {
        public readonly IBasketRepository _basketRepo;
        public readonly IProductRepository _productRepo;
        // public readonly IGenericRepository<DeliveryMethod> _dmRepo;
        public readonly IOrderRepository _orderRepo;
        public readonly IPaymentService _paymentService;


        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo, IBasketRepository basketRepo)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _productRepo.GetProductById(item.Id);
                var itemOrdered = new ProductItmeOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var OrderItem = new OrderItem(itemOrdered, productItem.Price,
                item.Quantity);
                items.Add(OrderItem);
            }
            var deliveryMethod = await _orderRepo.CreateOrder(deliveryMethodId);
            var subTotal = items.Sum(item => item.Price * item.Quantity);
			// var saveOrder = _orderRepo.SaveOrderToDb(order);
			try
			{
                var existOrder = await _orderRepo.GetOrderByPaymentId(basket.PaymentIntentId);
                if (existOrder !=null)
                {
                    var isDeletedOrder = _orderRepo.DeleteOrder(existOrder);
                    var isUpdated = await _orderRepo.CreateOrUpdatePaymentIntent(basket.Id);
                }
                 basket = await _basketRepo.GetBasketAsync(basketId);
                var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subTotal, basket.PaymentIntentId);
                var saveOrder = _orderRepo.SaveOrderToDb(order);
                return order;
            }
            catch (Exception ex)
			{

            }
            return null;
        }
        

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
           return await _orderRepo.GetDeliveryMethodsAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            return await _orderRepo.GetOrderByIdAsync(id, buyerEmail);
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            return await _orderRepo.GetOrdersForUserAsync(buyerEmail);
        }
    }
}