using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ILayoutService _layoutService;
        private readonly ISocialService _socialService;
        public FooterViewComponent(ILayoutService layoutService,
                                   ISocialService socialService)
        {
            _layoutService = layoutService;
            _socialService = socialService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM model = new()
            {
                Socials = await _socialService.GetSocials(),
                Settings = _layoutService.GetSettingsData()
            };

            return await Task.FromResult(View(model));
                                                             
        }
    }
}
