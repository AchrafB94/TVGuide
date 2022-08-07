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
                ViewBag.SearchQuery = query;
                return View(programmes);
        }
        public IActionResult Index()
        {
            List<Programme> tonightProgrammes = new List<Programme>();
            Channel randomChannel = new Channel();
            do
            {
                randomChannel = _channelRepository.getRandomChannel();
                if (!string.IsNullOrEmpty(randomChannel.IdXML))
                    tonightProgrammes = _channelRepository.GetTonightProgrammes(randomChannel.IdXML);
            }
            while (!tonightProgrammes.All(p => !string.IsNullOrEmpty(p.Image)) || !tonightProgrammes.Any());

            TonightViewModel model = new TonightViewModel();
            model.channel = randomChannel;
            model.programs = tonightProgrammes;
            return View(model);
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