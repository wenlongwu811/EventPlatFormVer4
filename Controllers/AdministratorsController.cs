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
    public class AdministratorsController : Controller
    {

        private AdministratorService administratorService;

        private readonly MvcEpfContext _context;

        public AdministratorsController(MvcEpfContext context)
        {
            _context = context;
            administratorService = new AdministratorService(context);
        }

        // GET: Administrators
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administrators.ToListAsync());
        }
        

        public async Task<IActionResult> Info(string id)
        {

            return View(await administratorService.GetEvents());

        }

        // GET: Administrators/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var administrator = await administratorService.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        //GET:Administrators/Verify:审核
        public async Task<IActionResult> Verify(string eventid,string admid)
        {
            ViewBag.id = admid;
           return View(await administratorService.EventInformation(eventid));
        }

        //:Administrators/Accept：接受未审核
        public async Task<IActionResult> Accept(string id, [Bind("State")] Event events)
        {
            await administratorService.Accept(id);
            return RedirectToAction(nameof(Info));
        }

        //Get:Administrator/Ban:禁止未审核
        public async Task<IActionResult>Ban(string id ,[Bind("State")] Event events)
        {
            await administratorService.Accept(id);
            return RedirectToAction(nameof(Info));
        }

        // GET: Administrators/Create:创建管理者
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleID,Name,Email,Phone,Pwd")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                await administratorService.Add(administrator);
                return RedirectToAction(nameof(Index));
            }
            return View(administrator);
        }

        // GET: Administrators/Edit/5
        public async Task<IActionResult> Edit(string? id)
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,RoleID,Name,Email,Phone,Pwd")] Administrator administrator)
        {
            if (id != administrator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                await administratorService.Update(administrator);
                return RedirectToAction(nameof(Index));
            }
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await administratorService.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await administratorService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
//wwl