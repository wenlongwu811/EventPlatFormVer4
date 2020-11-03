using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using EventPlatFormVer4.Data;
using EventPlatFormVer4.Models;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;

namespace EventPlatFormVer4.Controllers
{
    public class PeopleController : Controller
    {
        private readonly MvcEpfContext _context;

        public PeopleController(MvcEpfContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persons.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RoleID,Name,Email,Phone,Pwd")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("ID,RoleID,Name,Email,Phone,Pwd")] Person person)
        {
            if (id != person.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.ID))
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
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var person = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(uint id)
        {
            return _context.Persons.Any(e => e.ID == id);
        }
        // GET: People/Verify：可报名

        public async Task<IActionResult> Verify()
        {
            return View(await _context.Events.Where(m => m.State == 1).ToListAsync());

            //return View(await _context.Events.Where<m=>m.state=0>)
        }

        // GET: People/Apply:报名
        public async Task<IActionResult> Apply(uint eid, [Bind("ParticipatantState")] Event events)
        {
            var query = _context.Events.Where(m => m.Id == eid);
            //向Events_Participatants表中添加一条数据
            await _context.SaveChangesAsync();
            return View(await _context.Events.Where(m => m.State == 1).ToListAsync());
        }
        // GET: People/Withdraw:退赛
        public async Task<IActionResult> Withdraw(uint pid, [Bind("ParticipatantState")] Event_Participatant event_participatant)
        {
            var query = _context.Events_Participatant.Where(m => m.Pid == pid);
            query.First().ParticipatantState = 3;
            await _context.SaveChangesAsync();
            return View(await _context.Events_Participatant.Where(m => (m.ParticipatantState == 0|| m.ParticipatantState == 1)).ToListAsync());
        }

        // GET: People/Check:查成绩
        public async Task<IActionResult> Check(uint pid, [Bind("Grade")] Event events)
        {
            return View(await _context.Events_Participatant.Where(m => m.Pid == pid).ToListAsync());
        }


    }
}
