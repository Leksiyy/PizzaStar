using PizzaStar.Models.Pages;
using PizzaStar.Models;

namespace PizzaStar.Interfaces
{
    public interface IProduct
    {
        PagedList<Product> GetAllProducts(QueryOptions options);
        Task<Product> GetProductAsync(int id);
        Task<Product> GetProductWithCategoryAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task EditProductAsync(Product product);
    }
}
