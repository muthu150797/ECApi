using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using API.Specifications;
using EcAPI.Entities;
using EcAPI.Interfaces;
using EcAPI.Entity.OrderAggregrate;
using Microsoft.Extensions.Configuration;
using Stripe;
using EcAPI.Interfaces.Core.Interfaces;

namespace EcAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        // private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepo;


        public PaymentService(IBasketRepository basketRepository,IOrderRepository orderRepo, IConfiguration config)
        {
            _basketRepository = basketRepository;
            // _unitOfWork = unitOfWork;
            _config = config;
            _orderRepo = orderRepo;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string? basketId)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            var shippingPrice = 0m;

            if (basket == null) return null;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliverMethod = await _orderRepo.GetDeliveryMethodByIdAync(basket.DeliveryMethodId);//await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliverMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                // var productItem =new Product(); //await _unitOfWork.Repository<EcAPI.Entity.Product>().GetByIdAsync(item.Id);
                // if (item.Price != productItem.Price)
                // {
                //     item.Price = productItem.Price;
                // }
            }
            var service = new PaymentIntentService();
            PaymentIntent intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }

            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        // public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        // {
        //     var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
        //     var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        //     if (order == null) return null;

        //     order.Status = OrderStatus.PaymentReceived;
        //     _unitOfWork.Repository<Order>().Update(order);

        //     await _unitOfWork.Complete();

        //     return order;
        // }

        // public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        // {
        //     var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
        //     var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        //     if (order == null) return null;

        //     order.Status = OrderStatus.PaymentFailed;
        //     await _unitOfWork.Complete();

        //     return order;
        // }
    }
}