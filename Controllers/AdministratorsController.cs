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
    public class AdministratorsController : Controller
    {
        private readonly MvcEpfContext _context;

        public AdministratorsController(MvcEpfContext context)
        {
            _context = context;
        }

        // GET: Administrators
        public async Task<IActionResult> Index()
        {
            //return "You are an Administrator!";
            return View(await _context.Administrators.ToListAsync());
        }

        // GET: Administrators/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        //Get:Administrator/Alter:修改已审核的
        public async Task<IActionResult> Alter()
        {

            return View(await _context.Events.Where(m => m.State !=0).ToListAsync());

        }

        //GET:Administrators/Verify:待审核
        public async Task<IActionResult> Verify()
        {
            return View( await _context.Events.Where(m => m.State == 0).ToListAsync());

            //return View(await _context.Events.Where<m=>m.state=0>)
        }

        //:Administrators/Accept：接受
        public async Task<IActionResult> Accept(uint id, [Bind("State")] Event events)
        {
            var query = _context.Events.Where(m => m.Id == id);
            query.First().State = 1;
            await _context.SaveChangesAsync();
            return View(await _context.Events.Where(m => m.State == 0).ToListAsync());
        }

        //:Administrators/Accept2：接受
        public async Task<IActionResult> Accept2(uint id, [Bind("State")] Event events)
        {
            var query = _context.Events.Where(m => m.Id == id);
            query.First().State = 1;
            await _context.SaveChangesAsync();
            return View(await _context.Events.Where(m => m.State != 0).ToListAsync());
        }

        //Get:Administrator/Ban2:禁止
        public async Task<IActionResult> Ban2(uint id, [Bind("State")] Event events)
        {
            var query = _context.Events.Where(m => m.Id == id);
            query.First().State = 2;
            await _context.SaveChangesAsync();
            return View(await _context.Events.Where(m => m.State != 0).ToListAsync());
        }

        //Get:Administrator/Ban:禁止
        public async Task<IActionResult>Ban(uint id ,[Bind("State")] Event events)
        {
            var query = _context.Events.Where(m => m.Id == id);
            query.First().State = 2;
            await _context.SaveChangesAsync();
            return View(await _context.Events.Where(m => m.State == 0).ToListAsync());
        }

        private bool EventExists(uint id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        // GET: Administrators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleID,Name,Email,Phone,Pwd")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administrator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administrator);
        }

        // GET: Administrators/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,RoleID,Name,Email,Phone,Pwd")] Administrator administrator)
        {
            if (id != administrator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministratorExists(administrator.Id))
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
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var administrator = await _context.Administrators.FindAsync(id);
            _context.Administrators.Remove(administrator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministratorExists(uint id)
        {
            return _context.Administrators.Any(e => e.Id == id);
        }
    }
}
