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
            return View();
        }

        // GET: ChannelController/Details/5
        public ActionResult Details(string id)
        {
            Models.ViewModel viewModel = new ViewModel();
            viewModel.channel = channelRepository.getChannel(id);
            viewModel.programs = channelRepository.getPrograms(id);
            return View(viewModel);
        }

        // GET: ChannelController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChannelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChannelController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChannelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChannelController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChannelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
