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
    public class EventsController : Controller
    {
        private EventService eventService;
        private readonly MvcEpfContext _context;

        public EventsController(MvcEpfContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // TODO: add a view
        // GET: Events/GetE-Ps/5 显示Event的所有Participants
        public async Task<IActionResult> GetEventParticipants(string eventId)
        {
            if(eventId == null)
            {
                return NotFound();
            }
            ViewData["Event_Id"] = eventId;
            return View(await eventService.GetEventParticipantsAsync(eventId));
        }
        // GET: Events/GetE-Ps/5 显示Event的所有Participants
        public async Task<IActionResult> GetParticipantEvents(string participantId)
        {
            if (participantId == null)
            {
                return NotFound();
            }
            ViewData["Participant_Id"] = participantId;
            return View(await eventService.GetParticipantEventsAsync(participantId));
        }
        // GET: Events/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create: 创建新Event
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rank,EventStartTime,EventEndTime,SignUpStartTime,SignUpEndTime,Address,Detail")] Event @event)
        {
            if (ModelState.IsValid)
            {
                /*
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                */
                await eventService.AddEvent(@event);
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Rank,EventStartTime,EventEndTime,SignUpStartTime,SignUpEndTime,Address,Detail")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                    */
                    await eventService.UpdateEventParticipants(@event); // 更新 E-Participants
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            var @event = await eventService.FindEventAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string eventId)
        {
            /*
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            */
            await eventService.RemoveEventParticipants(eventId);
            await eventService.RemoveEvent(eventId);
            return RedirectToAction(nameof(Index));
        }


        private bool EventExists(string id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
