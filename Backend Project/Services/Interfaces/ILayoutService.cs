using Backend_Project.Models;
using Backend_Project.ViewModels;

namespace Backend_Project.Services.Interfaces
{
    public interface ILayoutService
    {
        Dictionary<string, string> GetSettingsData();
        Task<IEnumerable<Social>> GetSocialData();
    }
}
