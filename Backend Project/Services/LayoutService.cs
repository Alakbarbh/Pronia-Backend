using Backend_Project.Data;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;

namespace Backend_Project.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;
        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public LayoutVM GetSettingDatas()
        {
            Dictionary<string, string> settings = _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            return new LayoutVM { Settings = settings };
        }
    }
}
