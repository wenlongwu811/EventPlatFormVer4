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
            sponService = new SponsorService(context);
        }

        // GET: Sponsors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sponsors.ToListAsync());
        }

        //GET:Administrators/Show_Apply:详细信息
        [HttpGet]
        public async Task<IActionResult> Show_Apply(string eventid, string sid)
        {
            ViewData["sid"] = sid;
            return View(await sponService.EventInformation(eventid));
        }

        //GET:Administrators/Show_Verify:详细信息
        [HttpGet]
        public async Task<IActionResult> Show_Verify(string eventid, string sid)
        {
            ViewData["sid"] = sid;
            ViewData["eveid"] = eventid;
            var @event = await _context.EventParticipants.Where(item => item.Event_Id == eventid).ToListAsync();
            return View(@event);
        }

        public async Task<IActionResult> Info_Apply(string id)//申报活动，展示活动列表
        {
            ViewData["admid"] = id;
            var @event = await _context.Events.Where(item => item.SponsorId == id).ToListAsync();
            return View(@event);

        }
        public async Task<IActionResult> Info_Verify(string id)//审核参加，展示活动列表
        {
            ViewData["admid"] = id;
            var @event = await _context.Events.Where(item => item.SponsorId == id).ToListAsync();
            return View(@event);
        }


        // GET: Sponsors/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var sponsor = await sponService.FindAsync(id);

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
            var sponsor = await sponService.FindAsync(id);
            ViewData["aid"] = sponsor.Id;
            //var sponsor = sponService.Delete(id);
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
        public async Task<IActionResult> Apply(string id)
        {
            return RedirectToAction("Create", "Events",new {id=id });
        }

        //申请取消
        public async Task<IActionResult> Cancel(string id,string sid)
        {
            var _event = await _context.Events.Where(item => (item.Id == id)).FirstAsync();
            _event.State = 3;
            _context.SaveChanges();
            return RedirectToAction("Info_Apply", new { id = sid });
        }
    }
}
