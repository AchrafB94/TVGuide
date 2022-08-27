using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TVGuide.Models;

namespace TVGuide
{
    [Authorize]
    public class FavoriteChannelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TVGuideUser> _userManager;

        public FavoriteChannelsController(ApplicationDbContext context, UserManager<TVGuideUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FavoriteChannels
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _context.FavoriteChannels.Where(fc => fc.User == user).ToListAsync());
        }

        // GET: FavoriteChannels/Create
        public IActionResult Create()
        {
            ViewBag.Channels = _context.Channels.ToList();
            return View();
        }

        // POST: FavoriteChannels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FavoritePosition")] FavoriteChannel favoriteChannel, int IdChannel)
        {
            if (ModelState.IsValid)
            {
                favoriteChannel.Channel = _context.Channels.Where(ch => ch.Id == IdChannel).First();
                favoriteChannel.User = await _userManager.GetUserAsync(User);
                _context.Add(favoriteChannel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(favoriteChannel);
        }

        // GET: FavoriteChannels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FavoriteChannels == null)
            {
                return NotFound();
            }

            var favoriteChannel = await _context.FavoriteChannels.FindAsync(id);
            if (favoriteChannel == null)
            {
                return NotFound();
            }
            return View(favoriteChannel);
        }

        // POST: FavoriteChannels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FavoritePosition")] FavoriteChannel favoriteChannel)
        {
            if (id != favoriteChannel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteChannel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteChannelExists(favoriteChannel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(favoriteChannel);
        }

        // GET: FavoriteChannels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FavoriteChannels == null)
            {
                return NotFound();
            }
            
            var favoriteChannel = await _context.FavoriteChannels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteChannel == null)
            {
                return NotFound();
            }

            return View(favoriteChannel);
        }

        // POST: FavoriteChannels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FavoriteChannels == null)
            {
                return Problem("Entity set 'ChannelContext.FavoriteChannels'  is null.");
            }
            var favoriteChannel = await _context.FavoriteChannels.FindAsync(id);
            if (favoriteChannel != null)
            {
                _context.FavoriteChannels.Remove(favoriteChannel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteChannelExists(int id)
        {
          return _context.FavoriteChannels.Any(e => e.Id == id);
        }
    }
}
