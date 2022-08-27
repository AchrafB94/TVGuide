using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TVGuide.Models;

namespace TVGuide.Controllers
{
    [Authorize(Policy = "IsAdmin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
              return View(await _context.Channels.Include(ch => ch.Category).Include(ch => ch.Package).OrderBy(c => c.Position).ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Channels == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels.Include(ch => ch.Category).Include(ch => ch.Package).FirstOrDefaultAsync(ch =>ch.Id == id);
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Packages = _context.Packages.ToList();
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Position,Name,Logo,IdXML")] Channel channel, int IdPackage, int IdCategory)
        {
            if (ModelState.IsValid)
            {
                channel.Category = _context.Categories.First(c => c.Id == IdCategory);
                if(IdPackage != 0) channel.Package = _context.Packages.First(p => p.Id == IdPackage);
                _context.Add(channel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(channel);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Packages = _context.Packages.ToList();

            if (id == null || _context.Channels == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }
            return View(channel);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Position,Name,Logo,IdXML")] Channel channel, int IdPackage, int IdCategory)
        {
            if (id != channel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    channel.Category = _context.Categories.First(c => c.Id == IdCategory);
                    if (IdPackage != 0) channel.Package = _context.Packages.First(p => p.Id == IdPackage);
                    _context.Update(channel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChannelExists(channel.Id))
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
            return View(channel);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Channels == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Channels == null)
            {
                return Problem("Entity set 'ChannelContext.Channels'  is null.");
            }
            var channel = await _context.Channels.FindAsync(id);
            if (channel != null)
            {
                _context.Channels.Remove(channel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChannelExists(int id)
        {
          return _context.Channels.Any(e => e.Id == id);
        }
    }
}
