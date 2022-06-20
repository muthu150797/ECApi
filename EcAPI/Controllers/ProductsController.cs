using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using EcAPI.Entity;
using EcAPI.Interfaces;
using EcAPI.Helpers;
using EcAPI.Specification;
using AutoMapper;

namespace EcAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // public readonly AppIdentityDbContext _context;
        public readonly IProductRepository _productRepos;
        public readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _productsRepo;
        public readonly IConfiguration _config;

        public ProductsController(IConfiguration config,IGenericRepository<Product> productsRepo,IProductRepository productRepos)
        {
            // _context = context;
            _productRepos = productRepos;
            _productsRepo = productsRepo;
            _config = config;
        }
        [HttpPost]
        [Route("GetProducts")]
        public Task<IReadOnlyList<ProductToReturnDTO>> GetProducts()
        {
            return _productRepos.GetProducts();
        }
        //[HttpPost]
        [Route("GetAllProducts")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetAllProducts([FromQuery] ProductSpecParams productParams)
        {
            // Now takes brandID & TypeId properties;
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);
            var products = await _productsRepo.ListAsync(spec);
            List<ProductToReturnDTO> productList=new List<ProductToReturnDTO>();
            string path2 = Directory.GetCurrentDirectory();
            path2 = path2.Replace("\\", "/");

            foreach (var product in products)
			{
                ProductToReturnDTO prodList=new ProductToReturnDTO();
                prodList.Id = product.Id;
                prodList.Name = product.Name;
                prodList.Description = product.Description; 
                prodList.Price = product.Price;
                prodList.PictureUrl = _config["BaseUrl"] + product.PictureUrl;
                prodList.ProductBrand = product.ProductBrand.Name;
                prodList.ProductType= product.ProductType.Name;
                productList.Add(prodList);
			}
           // var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            return Ok(new Pagination<ProductToReturnDTO>(productParams.PageIndex, productParams.PageSize, totalItems,
                productList));
        }
        [HttpPost]
        [Route("SearchProducts")]
        public Task<IReadOnlyList<ProductToReturnDTO>> SearchProducts(string searchItem)
        {
            return _productRepos.SearchProducts(searchItem);
        }
        [HttpGet]
        [Route("SortProductByPrice")]
        public Task<IReadOnlyList<ProductToReturnDTO>> SortProductByPrice(string sortType)
        {
            return _productRepos.SortProductByPrice(sortType);
        }
        [HttpGet]
        [Route("FilterProducts")]
        public Task<IReadOnlyList<ProductToReturnDTO>> FilterProducts(int filterType, int productTypeId, int brandId)
        {
            return _productRepos.FilterProducts(filterType, productTypeId, brandId);
        }
        [HttpPost]
        [Route("Brands")]
        public Task<List<ProductBrand>> GetProductBrands()
        {
            return _productRepos.GetProductBrands();
        }
        [HttpPost]
        [Route("Types")]
        public Task<List<ProductType>> GetProductTypes()
        {
            return _productRepos.GetProductTypes();
        }
        [HttpGet]
        [Route("GetProductById/{id}")]
        public Task<ProductToReturnDTO> GetProductById(int id)
        {
            return _productRepos.GetProductById(id);
        }
    }
}