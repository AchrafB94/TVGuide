#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TVGuide.Models;

namespace TVGuide.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly ChannelContext _context;
        private readonly ProgrammeRepository _programmeRepository;

        public ChannelsController(ChannelContext context, ProgrammeRepository programmeRepository)
        {
            _context = context;
            _programmeRepository = programmeRepository;
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
            _programmeRepository.GetProgrammesByChannel(channel.IdXML, channel.XML);

            return View();
        }
    }
}
