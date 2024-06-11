using Microsoft.Extensions.Caching.Memory;
using ProductListWithCache5.Interfaces;
using ProductListWithCache5.Models;

namespace ProductListWithCache5.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "ProductList";
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public ProductRepo(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            if (!_cache.TryGetValue(_cacheKey, out List<Product> _))
            {
                var products = new List<Product>
                {
                    new Product { Id = 1, Name = "Coke", Price = 500M},
                    new Product { Id = 2, Name = "Fanta", Price = 500M}
                };
                _cache.Set(_cacheKey, products, _cacheOptions);
            }
        }
        public Product Create(Product product)
        {
            var products = GetAll();
            product.Id = products.Max(p => p.Id) + 1;
            products.Add(product);
            _cache.Set(_cacheKey, products, _cacheOptions);
            return product;
        }

        public bool Delete(int id)
        {
            var products = GetAll();
            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return false;
            }
            products.Remove(existingProduct);
            _cache.Set(_cacheKey, products, _cacheOptions);
            return true;
        }

        public List<Product> GetAll()
        {
            return _cache.Get<List<Product>>(_cacheKey);
        }

        public Product GetById(int id)
        {
            var products = GetAll();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public Product Update(Product product)
        {
            var products = GetAll();
            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            _cache.Set(_cacheKey, products, _cacheOptions);
            return existingProduct;
        }
    }
}
