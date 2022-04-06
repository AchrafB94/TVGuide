#nullable disable
using Microsoft.AspNetCore.Mvc;
using TVGuide.Models;

namespace TVGuide.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly ChannelContext _context;
        private readonly IChannelRepository _channelRepository;
        public ChannelsController(ChannelContext context, IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var channels = _context.Channels.ToList();
            return View(channels);
        }

        public IActionResult Details(int id)
        {
            ViewModel model = new ViewModel();
            Channel channel = _context.Channels.Where(ch => ch.Id == id).FirstOrDefault();
            model.programs = _channelRepository.GetProgrammesByChannel(channel.IdXML, channel.XML);
            model.channel = channel;
            return View(model);
        }
    }
}
