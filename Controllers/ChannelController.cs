using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TVGuide.Models;

namespace TVGuide.Controllers
{
    public class ChannelController : Controller
    {
        private readonly Models.IChannelRepository channelRepository;

        public ChannelController(Models.IChannelRepository channelRepository)
        {
            this.channelRepository = channelRepository;
        }
        // GET: ChannelController
        public ActionResult Index()
        {
            List<Channel> channels = channelRepository.getChannels().ToList();
            return View(channels);
        }

        public ActionResult Details(string id)
        {
            Models.ViewModel viewModel = new ViewModel();
            viewModel.channel = channelRepository.getChannel(id);
            viewModel.programs = channelRepository.getPrograms(id);
            return View(viewModel);
        }

    }
}
