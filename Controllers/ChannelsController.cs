#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TVGuide.Models;

namespace TVGuide.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly IChannelRepository _channelRepository;
        private readonly UserManager<TVGuideUser> _userManager;
        public ChannelsController(IChannelRepository channelRepository, UserManager<TVGuideUser> userManager)
        {
            _channelRepository = channelRepository;
            _userManager = userManager;
        }

        public IActionResult Index(int category)
        {
            List<Channel> channels;
            if(category != 0)
            {
                channels = _channelRepository.getChannelsByCategory(category);
                ViewBag.IdCategory = category;
                ViewBag.Header = _channelRepository.GetCategoryName(category);
            }
            else
                channels = _channelRepository.getAllChannels();

            return View(channels);
        }

        public IActionResult Details(int id)
        {
            ViewModel model = new ViewModel();
            Channel channel = _channelRepository.getChannel(id);
            model.programs = _channelRepository.GetProgrammesByChannel(channel.IdXML);
            model.channel = channel;
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var user = await _userManager.GetUserAsync(User);
            var userProgrammes = _channelRepository.GetUserProgrammes(user.Keywords);
            return View(userProgrammes);
        }

        public IActionResult Package(int id)
        {
            var channels = _channelRepository.getChannelsByPackage(id);
            return View(channels);
        }
    }
}
