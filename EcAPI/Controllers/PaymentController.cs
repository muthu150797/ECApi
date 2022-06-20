using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcAPI.Entities;
using EcAPI.Entity;
using Microsoft.AspNetCore.Authorization;
using EcAPI.Model;
using Stripe;
using EcAPI.Interfaces;

namespace EcAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PaymentController : ControllerBase
	{
		public readonly IOrderRepository _orderRepo;
		public PaymentController(IOrderRepository orderRepo)
		{
				_orderRepo = orderRepo;
		}
		//private readonly MyDBContext _context;
		// public PaymentController(MyDBContext context)
		// {
		// 	_context = context;
		// }

		[Authorize]
		[HttpPost("createOrUpdatePaymentIntent")]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string id)
		{
			var basket = await _orderRepo.CreateOrUpdatePaymentIntent(id);
			if (basket == null) return BadRequest( "Problem with your basket");
			return basket;
		}
		[HttpPost]
		[Route("CreateCharge")]
		public dynamic CreateCharge(StripeCharge payments)
		{
			//StripeConfiguration.SetApiKey("pk_test_51KtTUEAhDvR46CUdwBHiTB2LTtk0BDiNX1dLvolxHarH3KfTdY2CZx2egjVYKHlzlHxtjs9xUCDr9bksKbN0cKGe00fhyJeB8j");
			Stripe.StripeConfiguration.ApiKey = "sk_test_51KtTUEAhDvR46CUduakMZkBP9wr1CxNoiVniBNzsZqbA60KPi0Y0800U7sGiZn7gluJJS3kuGbjyBj6ixuSj37Bh00zLf9QokW";
			//var options = new TokenCreateOptions
			//{
			//	Card = new TokenCardOptions
			//	{
			//		Number = "4242424242424242",
			//		ExpMonth = 2,
			//		ExpYear = 2023,
			//		Cvc = "314",
			//	},
			//};
			var options = new CustomerCreateOptions
			{
				Email = payments.ReceiptEmail,
				Source = payments.Source
			};
			var service = new CustomerService();
			var cus = service.Create(options);
			var creditOptions = new ChargeCreateOptions
			{
				Amount = payments.Amount,// createOptions.Amount,
				Currency = payments.Currency,
				Customer = cus.Id,
				Description= payments.ProductName

			};
			var service3 = new ChargeService();
			var charge2 = service3.Create(creditOptions);
			return charge2.ToJson();
		}

		// [Authorize]
		// [HttpPost]
		// [Route("GetValue")]
		// public Entity.Product GetValue()
		// {
		// 	var response = _context.Products.Find(1);
		// 	return response;
		// }
	}
}