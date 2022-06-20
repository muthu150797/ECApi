using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entity;
using EcAPI.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EcAPI.Entity.OrderAggregrate;
using EcAPI.Model;

namespace EcAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<ProductToReturnDTO>> GetProducts();
        Task<IReadOnlyList<ProductToReturnDTO>> SearchProducts(string searchItem);
        Task<List<ProductBrand>> GetProductBrands();
        Task<List<ProductType>> GetProductTypes();
        public  Task<DeliveryMethod> CreateOrder(int id);
        Task<ProductToReturnDTO> GetProductById(int id);
        ResponseModel DeleteBrand(int id);
        ResponseModel AddOrUpdateProductType(ProductType type);
        ResponseModel DeleteProductType(int id);
        ResponseModel AddOrUpdateDeliveryMethod(DeliveryMethod dlMethod);
        ResponseModel DeleteDeliveryMethod(int id);
        ResponseModel AddOrUpdateBrands(ProductBrand brands);
        Task<IReadOnlyList<ProductToReturnDTO>> SortProductByPrice(string sortType);
        Task<IReadOnlyList<ProductToReturnDTO>> FilterProducts(int filterType,int productTypeId,int brandId);

    }
}