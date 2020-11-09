using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using EventPlatFormVer4.Data;
using EventPlatFormVer4.Models;
using EventPlatFormVer4.Service;

namespace EventPlatFormVer4.Controllers
{
    public class SponsorsController : Controller
    {
        private readonly MvcEpfContext _context;

        public SponsorService sponService;

        public SponsorsController(MvcEpfContext context)
        {
            _context = context;
            sponserv = new SponsorService(context);
        }

        // GET: Sponsors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sponsors.ToListAsync());
        }

        // GET: Sponsors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await sponService.Find(id);

            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // GET: Sponsors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sponsors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleID,Name,Email,Phone,Pwd,Certificate")] Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(sponsor);
                await sponService.Add(sponsor);
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }

        // GET: Sponsors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor == null)
            {
                return NotFound();
            }
            return View(sponsor);
        }

        // POST: Sponsors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,RoleID,Name,Email,Phone,Pwd,Certificate")] Sponsor sponsor)
        {
            if (id != sponsor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorExists(sponsor.Id))
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
            return View(sponsor);
        }

        // GET: Sponsors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*
            var sponsor = await _context.Sponsors
                .FirstOrDefaultAsync(m => m.Id == id);
            */
            var sponsor = sponService.Delete(id);

            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // POST: Sponsors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sponsor = await _context.Sponsors.FindAsync(id);
            _context.Sponsors.Remove(sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool sponsorExists(string id)

        {
            return _context.Sponsors.Any(e => e.Id == id);
        }

        private bool SponsorExists(string id)
        {
            return _context.Sponsors.Any(e => e.Id == id);
        }

        //申报
        public async Task<IActionResult> Apply(Event _event, string id)//前端是如何让传入这个EP的呢
        {
            await sponService.Apply(_event, id);
            return View(_context.Events.Where(item => !item.Equals(_event) && item.State == 1));
        }

        //申请取消
        public async Task<IActionResult> Cancel(string? id, [Bind("State")] Event _event)//前端是如何让传入这个EP的呢
        {
            await sponService.Cancel(_event, id);
            return View(sponService.ApplyEvents(id));
        }
    }
}
