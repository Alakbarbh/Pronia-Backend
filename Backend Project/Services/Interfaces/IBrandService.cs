using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrands();
    }
}
