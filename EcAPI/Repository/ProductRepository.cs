using EcAPI.Entity;
using Microsoft.EntityFrameworkCore;
using EcAPI.Entities;
using EcAPI.Interfaces;
using System.Data;
using AutoMapper;
using EcAPI.Entity.OrderAggregrate;
using EcAPI.Model;

namespace EcAPI.Repository
{

    public class ProductRepository : IProductRepository
    {
        public readonly AppIdentityDbContext _context;
        public readonly IMapper _mapper;
        public readonly IConfiguration _config;
        public ProductRepository(IConfiguration config, AppIdentityDbContext context, IMapper mapper)
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
             s.Price.ToString() == searchItem ||
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
            try
            {
                var response = await _context.Products.Where(x => x.Id == id)
                    .Include(x => x.ProductType)
                    .Include(x => x.ProductBrand).FirstAsync();
                // response.PictureUrl =  response.PictureUrl;
                return _mapper.Map<Product, ProductToReturnDTO>(response);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<DeliveryMethod> CreateOrder(int id)
        {
            var response = await _context.DeliveryMethods.Where(x => x.Id == id).FirstAsync();
            return _mapper.Map<DeliveryMethod, DeliveryMethod>(response);
        }

        public ResponseModel AddOrUpdateBrands(ProductBrand brands)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (brands.Id == 0)
                {
                    _context.ProductBrands.Add(brands);
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The brand added successfully";
                }
                else
                {
                    var entity = _context.ProductBrands.FirstOrDefault(x => x.Id == brands.Id);
                    entity.Name = brands.Name;
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The brand updated successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public ResponseModel DeleteBrand(int id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var entity = _context.ProductBrands.FirstOrDefault(x => x.Id == id);
                _context.Remove(entity);
                _context.SaveChanges();
                response.StatusCode = 200;
                response.Message = "The brand deleted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public ResponseModel AddOrUpdateProductType(ProductType type)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (type.Id == 0)
                {
                    _context.ProductType.Add(type);
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The type added successfully";
                }
                else
                {
                    var entity = _context.ProductType.FirstOrDefault(x => x.Id == type.Id);
                    entity.Name = type.Name;
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The type updated successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public ResponseModel DeleteProductType(int id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var entity = _context.ProductType.FirstOrDefault(x => x.Id == id);
                _context.Remove(entity);
                _context.SaveChanges();
                response.StatusCode = 200;
                response.Message = "The product type deleted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public ResponseModel AddOrUpdateDeliveryMethod(DeliveryMethod dlMethod)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (dlMethod.Id == 0)
                {
                    _context.DeliveryMethods.Add(dlMethod);
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The delivery method added successfully";
                }
                else
                {
                    var entity = _context.DeliveryMethods.FirstOrDefault(x => x.Id == dlMethod.Id);
                    entity.DeliveryTime = dlMethod.DeliveryTime;
                    entity.ShortName = dlMethod.ShortName;
                    entity.Description = dlMethod.Description;
                    entity.Price = dlMethod.Price;
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The delivery method updated successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public ResponseModel DeleteDeliveryMethod(int id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var entity = _context.DeliveryMethods.FirstOrDefault(x => x.Id == id);
                _context.Remove(entity);
                _context.SaveChanges();
                response.StatusCode = 200;
                response.Message = "The delivery method deleted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public ResponseModel AddOrUpdateProduct(List<Product> product)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                foreach (var item in product)
                {
                    if (item.Id == 0)
                    {
                        _context.Products.Add(item);
                        response.StatusCode = 200;
                        response.Message = "The products added successfully";
                    }
                    else
                    {
                        var entity = _context.Products.FirstOrDefault(x => x.Id == item.Id);
                        entity.Name = item.Name;
                        entity.Price = item.Price;
                        entity.Description = item.Description;
                        entity.PictureUrl = item.PictureUrl;
                        entity.ProductBrandId = item.ProductBrandId;
                        entity.ProductTypeId = item.ProductTypeId;
                        response.StatusCode = 200;
                        response.Message = "The product updated successfully";
                    }
                }
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public ResponseModel DeleteProduct(int id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var entity = _context.Products.FirstOrDefault(x => x.Id == id);
                _context.Remove(entity);
                _context.SaveChanges();
                response.StatusCode = 200;
                response.Message = "The product deleted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 401;
                response.Message = ex.ToString();
            }
            return response;
        }
        public FileUploadModel Upload(List<IFormFile> files)
        { 
             FileUploadModel response=new FileUploadModel();
           List<string> allPicPaths=new List<string>();
            var directory = Directory.GetCurrentDirectory().Replace("\\", "/");
            try
            {
                foreach (var file in files)
                {

                    string path = directory + "/Content/images/products/" + file.FileName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        response.fileUrls +=path;
                        allPicPaths.Add(path);
                    }
                }
            }
            catch(Exception ex)
            {
            return response;
            }
            return response;
        }

    }

    public class FileUploadModel
    {
     public string fileUrls{get;set;}
    }
}