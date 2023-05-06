using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_Slider.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ILayoutService _layoutService;
        public HeaderViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            LayoutVM model = new()
            {
                Settings = _layoutService.GetSettingsData()
            };

            return await Task.FromResult(View(model));
        }
    }
}
