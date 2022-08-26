using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
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
            
            Channel randomChannel = _channelRepository.getRandomChannel();
            if (!string.IsNullOrEmpty(randomChannel.IdXML))
                tonightProgrammes = _channelRepository.GetTonightProgrammes(randomChannel.IdXML);

            TonightViewModel model = new TonightViewModel();
            model.channel = randomChannel;
            model.programs = tonightProgrammes;
            var channels = _channelRepository.getAllChannels().OrderBy(ch => ch.Name).ToList();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [HttpPost] public IActionResult CultureManagement(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30)});
            return RedirectToAction(nameof(Index));
        }
    }
}