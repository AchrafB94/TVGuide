using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TVGuide.Models;

namespace TVGuide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChannelRepository _channelRepository;

        public HomeController(ILogger<HomeController> logger, IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
            _logger = logger;
        }

        public IActionResult Index(string search)
        {
            if(!string.IsNullOrEmpty(search) && search.Length > 3)
            {
                List<Programme> programmes = _channelRepository.GetProgrammesByNameAndDescription(search);
                return View(programmes);
            }
            else
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}