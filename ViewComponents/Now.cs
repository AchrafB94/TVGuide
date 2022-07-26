using Microsoft.AspNetCore.Mvc;
using TVGuide.Models;

namespace TVGuide.Views.Home
{
    public class Now : ViewComponent
    {
        private readonly IChannelRepository _repository;

        public Now(IChannelRepository repository) => _repository = repository;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_repository.GetCurrentProgrammes());
        }
    }
}
