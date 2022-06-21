using AutoMapper;
using EcAPI.Entities;
using EcAPI.Entity.OrderAggregrate;
using EcAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _context;
		private readonly Entity.AppIdentityDbContext _dbContext;
		private readonly IConfiguration _config;
		public OrdersController(IConfiguration config,Entity.AppIdentityDbContext dbContext, IHttpContextAccessor context, IMapper mapper, IOrderService orderService)
		{
			_orderService = orderService;
			_mapper = mapper;
			_context = context;
				_config = config;
			_dbContext = dbContext;
		}
		// public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		// {
		//     var email = HttpContext.User.RetriveEmailFromPrincipal();
		//     var address = _mapper.Map<AddressDto,Address>(orderDto,ShipToAddress);
		// }
		[Authorize]
		[HttpPost]
		[Route("CreateOrder")]
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var email = _context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

			// var email = HttpContext.User.RetriveEmailFromPrincipal();
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);
			var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);
			if (order == null) return order;// BadRequest(new ApiResponse(400,"Problem creating order"));
			return Ok(order);
		}
		[HttpGet]
		[Route("GetOrdersForUserAsync")]
		public async Task<ActionResult<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);
			return Ok(orders);
		}
		[Authorize]
		[HttpGet]
		[Route("GetOrdersForUser")]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser()
		{
			var email = _context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
			var orderList = new List<OrderToReturnDto>();
			try
			{
				var orders = await _orderService.GetOrdersForUserAsync(email);
				foreach (var order in orders)
				{
					orderList.Add(new OrderToReturnDto()
					{
						Id = order.Id,
						BuyerEmail = order.BuyerEmail,
						OrderDate = order.OrderDate,
						Status = order.Status.ToString(),
						Subtotal = order.Subtotal,
						Total = order.Subtotal + _dbContext.DeliveryMethods.Where(x => x.Id == order.DeliveryMethodId).Select(x => x.Price).First()
					});
				}
			}
			catch (Exception ex)
			{

			}
			
			return Ok(orderList);
		}
		[HttpGet()]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUsers(int id)
		{
			var email = _context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
			var order = await _orderService.GetOrderByIdAsync(id, email);
			var order2 = new OrderToReturnDto()
			{
				Id = order.Id,
				BuyerEmail = order.BuyerEmail,
				OrderDate = order.OrderDate,
				Status = order.Status.ToString(),
				Subtotal = order.Subtotal,
				//   ShippingPrice=order.sh
			};
			// order2.
			if (order == null) return null;// BadRequest(new ApiResponse(,"Problem creating order"));
			return order2;// _mapper.Map<Order, OrderToReturnDto>(order);
		}
		[HttpGet()]
		[Route("GetOrderByIdForUser")]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
		{
			var email = _context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
			var order = await _orderService.GetOrderByIdAsync(id, email);

			var orderItems = _dbContext.OrderItems.Where(x => x.OrderId == order.Id).ToList();
			List<ProductItmeOrdered> ItemList =new List<ProductItmeOrdered>();
			foreach (var items in orderItems)
			{
				//items.ItmeOrdered.PictureUrl=  items.ItmeOrdered.PictureUrl;
				items.Id = 0;
			}
			var order2 = new OrderToReturnDto()
			{
				Id = order.Id,
				BuyerEmail = order.BuyerEmail,
				OrderDate = order.OrderDate,
				Status = order.Status.ToString(),
				Subtotal = order.Subtotal,
				ShippingPrice = _dbContext.DeliveryMethods.Where(x => x.Id == order.DeliveryMethodId).Select(x => x.Price).First(),
				Total = order.Subtotal + _dbContext.DeliveryMethods.Where(x => x.Id == order.DeliveryMethodId).Select(x => x.Price).First(),
				OrderItems = orderItems
			};
			if (order == null) return null;
			return order2;// _mapper.Map<Order, OrderToReturnDto>(order);
		}
		[HttpGet]
		[Route("GetDeliveryMethods")]
		public async Task<ActionResult<Order>> GetDeliveryMethodsAsync()
		{
			var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
			return Ok(deliveryMethods);
		}

	}
}