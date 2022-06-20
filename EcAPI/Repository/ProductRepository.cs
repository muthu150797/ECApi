using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using EcAPI.Entities;
using EcAPI.Interfaces;
using System.Data;
using AutoMapper;
using EcAPI.Entity.OrderAggregrate;

namespace EcAPI.Repository
{

    public class ProductRepository : IProductRepository
    {
        public readonly AppIdentityDbContext _context;
        public readonly IMapper _mapper;
        public readonly IConfiguration _config;
        public ProductRepository(IConfiguration config,AppIdentityDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }
        public async Task<IReadOnlyList<ProductToReturnDTO>> GetProducts()
        {
            var response = await _context.Products
            .Include(x => x.ProductType)
            .Include(x => x.ProductBrand).ToListAsync();
            return _mapper.Map<List<Product>, List<ProductToReturnDTO>>(response);
        }
        public async Task<IReadOnlyList<ProductToReturnDTO>> SortProductByPrice(string sortType)
        {
            List<Product> response = new List<Product>();

            if (sortType == "ascending")
            {
                response = await _context.Products
           .Include(x => x.ProductType)
           .Include(x => x.ProductBrand).OrderBy(x => x.Price).ToListAsync();
            }
            else
            {
                response = await _context.Products
            .Include(x => x.ProductType)
            .Include(x => x.ProductBrand).OrderByDescending(x => x.Price).ToListAsync();
            }

            return _mapper.Map<List<Product>, List<ProductToReturnDTO>>(response);
        }
        public async Task<IReadOnlyList<ProductToReturnDTO>> SearchProducts(string searchItem)
        {
            List<Product> response = new List<Product>();
            response = await _context.Products
            .Include(x => x.ProductType).Include(x => x.ProductBrand)
            .Where(s =>
             s.Name.ToLower().Contains(searchItem.ToLower()) ||
             s.Price.ToString() == searchItem||
             s.Description.ToLower().Contains(searchItem.ToLower())).ToListAsync();
            //Where(t => t.Contains(searchItem));
            return _mapper.Map<List<Product>, List<ProductToReturnDTO>>(response);
        }
        public async Task<IReadOnlyList<ProductToReturnDTO>> FilterProducts(int filterType, int productTypeId, int brandId)
        {
            List<Product> response = new List<Product>();

            if (filterType == 1)//filter by productType
            {
                response = await _context.Products
                .Include(x => x.ProductType)
                .Include(x => x.ProductBrand).Where(x => x.ProductTypeId == productTypeId).ToListAsync();
            }
            if (filterType == 2)//filter by brand
            {
                response = await _context.Products
                .Include(x => x.ProductType)
                .Include(x => x.ProductBrand).Where(x => x.ProductBrandId == brandId).ToListAsync();
            }
            if (filterType == 3)//filter by both productTypr and brand
            {
                response = await _context.Products
                .Include(x => x.ProductType)
                .Include(x => x.ProductBrand)
                .Where(x => x.ProductTypeId == productTypeId && x.ProductBrandId == brandId).ToListAsync();
            }
            return _mapper.Map<List<Product>, List<ProductToReturnDTO>>(response);
        }

        public async Task<List<ProductBrand>> GetProductBrands()
        {
            var response = await _context.ProductBrands.ToListAsync();
            return response;
        }

        public async Task<List<ProductType>> GetProductTypes()
        {
            var response = await _context.ProductType.ToListAsync();
            return response;
        }
        public async Task<ProductToReturnDTO> GetProductById(int id)
        {
            var response = await _context.Products.Where(x => x.Id == id)
            .Include(x => x.ProductType)
            .Include(x => x.ProductBrand).FirstAsync();
            response.PictureUrl=_config["BaseUrl"]+response.PictureUrl;
            return _mapper.Map<Product, ProductToReturnDTO>(response);
        }
        public async Task<DeliveryMethod> CreateOrder(int id)
        {
            var response = await _context.DeliveryMethods.Where(x => x.Id == id).FirstAsync();
            return _mapper.Map<DeliveryMethod, DeliveryMethod>(response);
        }
    }
}