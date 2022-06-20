using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcAPI.Entity;
using EcAPI.Entity.OrderAggregrate;
using EcAPI.Interfaces;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Stripe;
using EcAPI.Interfaces.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using EcAPI.Entities;

namespace EcAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public readonly AppIdentityDbContext _context;
        public readonly IMapper _mapper;
        public readonly IConfiguration _config;
        public readonly IProductRepository _productRepo;
        public readonly IBasketRepository _basketRepo;

        public OrderRepository(IProductRepository productRepo,IBasketRepository basketRepo, AppIdentityDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _basketRepo = basketRepo;
            _productRepo = productRepo;
        }
        public async Task<DeliveryMethod> CreateOrder(int id)
        {
            var response = await _context.DeliveryMethods.AsNoTracking().Where(x => x.Id == id).FirstAsync();
            return _mapper.Map<DeliveryMethod, DeliveryMethod>(response);
        }
        public async Task<Entity.OrderAggregrate.Order> GetOrderByPaymentId(string id)
        {
            
            var response = await _context.Orders.AsNoTracking().Where(x => x.PaymentIntentId == id).FirstOrDefaultAsync();

            return response;
        }
        public async Task<DeliveryMethod> GetDeliveryMethodByIdAync(int? id)
        {
            return await _context.DeliveryMethods.AsNoTracking().Where(x => x.Id == id).FirstAsync();
        }
        public int DeleteOrder(Entity.OrderAggregrate.Order order)
        {
            try
            {
                _context.Entry(order).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 401;
            }
            return 200;
        }

        public dynamic SaveOrderToDb(Entity.OrderAggregrate.Order order)
        {
            try
            {
                //var group = _context.Orders.First(g => g.Id == model.Group.Id);
                // _context.Entry(group).CurrentValues.SetValues(model.Group);
                // _context.SaveChangesAsync();
                //var group = _context.DeliveryMethods.First(g => g.Id == order.DeliveryMethod.Id);
                _context.ChangeTracker.TrackGraph(order, node =>node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);
                _context.Orders.Add(order);
                _context.SaveChanges();
                //  _context.SaveChangesAsync();
                //_context.Orders.Add(order);
                // _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex;
            }
            return 200;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);
            var shippingPrice = 0m;

            if (basket == null) return null;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliverMethod = await GetDeliveryMethodByIdAync(basket.DeliveryMethodId);//await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliverMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await _productRepo.GetProductById(item.Id);//await _unitOfWork.Repository<EcAPI.Entity.Product>().GetByIdAsync(item.Id);

				 if (item.Price != productItem.Price)
				{
					item.Price = productItem.Price;
				}
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
                    Description= basket.Id,
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }
        public async Task<Entity.OrderAggregrate.Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var response = await _context.Orders.Where(o => o.Id==id&& o.BuyerEmail == buyerEmail).FirstAsync();
            return response;
        }

        public async Task<IReadOnlyList<Entity.OrderAggregrate.Order>> GetOrdersForUserAsync(string buyerEmail)
		{
            var response=await _context.Orders.Where(o => o.BuyerEmail == buyerEmail).ToListAsync();
            return response;
		}
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var response = await _context.DeliveryMethods.ToListAsync();
            return response;
        }
    }
}