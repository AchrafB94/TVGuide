using Microsoft.AspNetCore.Mvc;
using TVGuide.Models;

namespace TVGuide.Views.Home
{
    public class Now : ViewComponent
    {
        private readonly IChannelRepository _repository;

        public Now(IChannelRepository repository) => _repository = repository;

        public async Task<IViewComponentResult> InvokeAsync(int IdCategory)
        {
            if (IdCategory == 0)
                return View(await _repository.GetCurrentProgrammes());
            else
                return View(await _repository.GetCurrentProgrammes(IdCategory));
        }
    }
}
