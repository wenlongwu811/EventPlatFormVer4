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
    public class ParticipantsController : Controller
    {
        private ParticipantService participantService;

        private readonly MvcEpfContext _context;

        public ParticipantsController(MvcEpfContext context)
        {
            _context = context;
            participantService = new ParticipantService(context);
        }

        // GET: participants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Participants.ToListAsync());
        }


        //参赛者主界面
        public async Task<IActionResult> Info(string id)
        {
            return View(await participantService.FindEvent(id));
        }

        //参赛者个人信息
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var participant = await participantService.Find(id);

            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // GET: participants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: participants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleID,Name,Email,PhoneNum,Passwd")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                await participantService.Add(participant);
                return RedirectToAction(nameof(Index));
            }
            return View(participant);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }
            return View(participant);
        }

        // POST: participants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(string id, [Bind("Id,RoleID,Name,Email,PhoneNum,Passwd")] Participant participant)

        {
            if (id != participant.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!participantExists(participant.ID))
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
            return View(participant);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await participantService.Find(id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await participantService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool participantExists(string id)

        {
            return _context.Participants.Any(e => e.ID == id);
        }

        //报名
        public async Task<IActionResult> Apply(EventParticipant EP,string id)//前端是如何让传入这个EP的呢
        {
            await participantService.Apply(EP,id);
            return View(_context.Events.Where(item=>!item.Equals(EP)&&item.State==1));
        }
        //退赛
        public async Task<IActionResult> ExitEvent(string id, [Bind("State")] EventParticipant EP)//前端是如何让传入这个EP的呢
        {
            await participantService.ExistEvent(EP, id);
            return View(participantService.FindEvent(id));
        }

    }
}
