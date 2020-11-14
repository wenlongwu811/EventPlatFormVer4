using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlatFormVer4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        /* private readonly MvcEpfContext _context;

        public HomeController(MvcEpfContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Find(string id, string pwd, string role)
        {
            if (role == "0")
            {
                using (var db = _context)
                {
                    var administrator = await db.Administrators.Where(item => item.Id == id && item.Pwd == pwd).FirstOrDefaultAsync();
                    if (administrator != null) return RedirectToAction(nameof(Details_Adm));
                    else return View();
                }

            }
            else if (role == "1")
            {
                using (var db = _context)
                {
                    var sponsor = await db.Sponsors.Where(item => item.Id == id && item.Pwd == pwd).FirstOrDefaultAsync();
                    if (sponsor != null) return RedirectToAction(nameof(Details_Spo));
                    else return View();
                }
            }
            else
            {
                using (var db = _context)
                {
                    var participant = await db.Participants.Where(item => item.ID == id && item.PassWd == pwd).FirstOrDefaultAsync();
                    if (participant != null) return RedirectToAction(nameof(Details_Par));
                    else return View();
                }
            }

        }

        public IActionResult Details_Adm(Administrator administrator)
        {

            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        public IActionResult Details_Spo(Sponsor sponsor)
        {

            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        public IActionResult Details_Par(Participant participant)
        {

            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
