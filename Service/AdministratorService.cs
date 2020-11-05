using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EventPlatFormVer4.Service
{
    public class AdministratorService
    {
        private static MvcEpfContext _context;
        public AdministratorService(MvcEpfContext context)
        {
            _context = context;
        }

        public void Add(Administrator administrator)
        {
            try
            {
                using (var db =_context)
                {
                    db.Administrators.Add(administrator);
                    db.SaveChanges();
                }
                
            }
            catch (Exception e)
            {
                throw new ApplicationException("添加失败");
            }
        }
        
        public void Delete(string id)
        {
            using (var db = _context)
            {
                var administrator = db.Administrators.Where(item => item.Id == id);
                db.Administrators.RemoveRange(administrator);
                db.SaveChanges();
            }
        }

        public Administrator Find(string id)
        {
            using (var db = _context)
            {
                var adminitrator = db.Administrators.Where(item => item.Id == id);
                return (Administrator)adminitrator;//TODO添加显式转换
            }
        }

         public void Update(Administrator administrator)
        {
            using (var db = _context)
            {
                db.Update(administrator);
                db.SaveChanges();
            }
        }

        public void Accept(string id)
        {
            using (var db = _context)
            {
                Event @event =(Event) db.Events.Where(item => item.Id == id);
                @event.State = 1;
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }

        public void Deny(string id)
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                @event.State = 2;
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }

        public Event Verify(string id)
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                return @event;
            }
        }

        public Event Alter(string id)
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                return @event;
            }
        }

        public List<Event> GetEvents(int state)
        {
            using (var db = _context)
            {
                var query = db.Events.Where(item => item.State == state);
                return query.ToList();
            }
        }
    }
}
