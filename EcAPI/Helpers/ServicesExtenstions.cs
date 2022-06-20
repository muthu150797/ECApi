using EcAPI.Entities;
using EcAPI.Entity;
using EcAPI.Interfaces;
using EcAPI.Repository;
using EcAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using EcAPI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using EcAPI.Interfaces.Core.Interfaces;
using EcAPI.Repository;

namespace EcAPI.Helpers
{
    public static class ServicesExtensions
    {

        public static void AddMyLibraryServices(this IServiceCollection services)
        {

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            //services.AddScoped<IPaymentService, PaymentService>();
        }
    }
}