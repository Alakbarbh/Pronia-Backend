using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetById(int id);
        Task<List<Product>> GetAll();
        Task<int> GetCountAsync();
        Task<List<Product>> GetFeaturedProducts();
        Task<List<Product>> GetBestsellerProducts();
        Task<List<Product>> GetLatestProducts();
        Task<List<Product>> GetNewProducts();

        Task<Product> GetFullDataById(int id);
        Task<List<Product>> GetPaginateDatas(int page, int take);
    }
}
