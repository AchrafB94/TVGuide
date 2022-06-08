#nullable disable
using Microsoft.AspNetCore.Mvc;
using TVGuide.Models;

namespace TVGuide.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly IChannelRepository _channelRepository;
        public ChannelsController(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public IActionResult Index(string package, string category)
        {
            List<Channel> channels;
            if(package != null)
                channels = _channelRepository.getChannelsByPackage(package);
            else if(category != null)
                channels = _channelRepository.getChannelsByCategory(category);
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
    }
}
