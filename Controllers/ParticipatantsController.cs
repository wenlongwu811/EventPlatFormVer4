     using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using EventPlatFormVer4.Data;
using EventPlatFormVer4.Models;

namespace EventPlatFormVer4.Controllers
{
    public class ParticipatantsController : Controller
    {
        private readonly MvcEpfContext _context;

        public ParticipatantsController(MvcEpfContext context)
        {
            _context = context;
        }

        // GET: Participatants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Participatants.ToListAsync());
        }

        // GET: Participatants/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participatant = await _context.Participatants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (participatant == null)
            {
                return NotFound();
            }

            return View(participatant);
        }

        // GET: Participatants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Participatants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleID,Name,Email,Phone,Pwd")] Participant participatant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participatant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(participatant);
        }

        // GET: Participatants/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participatant = await _context.Participatants.FindAsync(id);
            if (participatant == null)
            {
                return NotFound();
            }
            return View(participatant);
        }

        // POST: Participatants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,RoleID,Name,Email,Phone,Pwd")] Participant participatant)
        {
            if (id != participatant.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participatant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipatantExists(participatant.ID))
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
            return View(participatant);
        }

        // GET: Participatants/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participatant = await _context.Participatants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (participatant == null)
            {
                return NotFound();
            }

            return View(participatant);
        }

        // POST: Participatants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var participatant = await _context.Participatants.FindAsync(id);
            _context.Participatants.Remove(participatant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipatantExists(string id)
        {
            return _context.Participatants.Any(e => e.ID == id);
        }
    }
}
