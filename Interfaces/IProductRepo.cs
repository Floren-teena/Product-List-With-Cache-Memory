using ProductListWithCache5.Models;

namespace ProductListWithCache5.Interfaces
{
    public interface IProductRepo
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product Create(Product product);
        Product Update(Product product);
        bool Delete(int id);
    }
}
