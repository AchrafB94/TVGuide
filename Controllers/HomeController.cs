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

        public IActionResult Search(string query)
        {
                List<Programme> programmes = _channelRepository.GetProgrammesByNameAndDescription(query);
                return View(programmes);
        }

        public IActionResult Index()
        {
            var programmes = _channelRepository.GetCurrentProgrammes();
            return View(programmes);
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