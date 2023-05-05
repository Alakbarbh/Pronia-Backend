using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetById(int id);
        Task<List<Product>> GetAll();
        Task<int> GetCountAsync();


        //Task<Product> GetFullDataById(int id);
        //Task<List<Product>> GetPaginateDatas(int page, int take);
    }
}
