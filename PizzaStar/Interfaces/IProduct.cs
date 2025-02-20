using PizzaStar.Models.Pages;
using PizzaStar.Models;

namespace PizzaStar.Interfaces
{
    public interface IProduct
    { 
        Task<IEnumerable<Product>> GetEightRandomProductsAsync(int productId);
        PagedList<Product> GetAllProductsByCategory(QueryOptions options, int categoryId);
        PagedList<Product> GetAllProducts(QueryOptions options);
        Task<Product> GetProductAsync(int id);
        Task<Product> GetProductWithCategoryAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task EditProductAsync(Product product);
    }
}
