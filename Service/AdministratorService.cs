using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using SQLitePCL;
using System.Security.Cryptography.X509Certificates;

namespace EventPlatFormVer4.Service
{
    public class AdministratorService
    {
        private static MvcEpfContext _context;
        public AdministratorService(MvcEpfContext context)
        {
            _context = context;
        }

        public async Task Add(Administrator administrator)
        {
            try
            {
                using (var db = _context)
                {
                    db.Administrators.Add(administrator);
                   await db.SaveChangesAsync();
                }

            }
            catch (Exception)
            {
                throw new ApplicationException("添加失败");
            }
        }

        public async Task Delete(string id)
        {
            using (var db = _context)
            {
                var administrator =await db.Administrators.Where(item => item.Id == id).FirstOrDefaultAsync();
                db.Administrators.RemoveRange(administrator);
                db.SaveChanges();
            }
        }

        public async Task<Administrator> FindAsync(string id)
        {
            using (var db = _context)
            {
                var adminitrator =await db.Administrators.Where(item => item.Id == id).FirstOrDefaultAsync();
                return adminitrator;//TODO添加显式转换
            }
        }

        public async Task<string> loading(string name,string pwd)
        {
            using (var db = _context)
            {
                var adminitrator = await db.Administrators.Where(item => item.Name == name&&item.Pwd==pwd).FirstOrDefaultAsync();
                if (adminitrator==null) return null;
                else return adminitrator.Id;
            }
        }

        public async Task Update(Administrator administrator)
        {
            using (var db = _context)
            {
                db.Update(administrator);
               await db.SaveChangesAsync();
            }
        }

        public async Task Accept(string id)
        {
            using (var db = _context)
            {
                Event @event =await db.Events.Where(item => item.Id == id).FirstOrDefaultAsync();
                @event.State = 1;
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }

        public async Task Deny(string id)
        {
            using (var db = _context)
            {
                Event @event = await db.Events.Where(item => item.Id == id).FirstOrDefaultAsync();
                @event.State = 2;
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }

        public async Task<Event> Verify(string id)//只有一个信息的方法
        {
            using (var db = _context)
            {
                Event @event =await db.Events.Where(item => item.Id == id).FirstOrDefaultAsync();
                return @event;
            }
        }

        public async Task<List<Event>> GetEvents()
        {
            using (var db = _context)
            {
                var query = await db.Events.Where(item => item.Id != null).ToListAsync();
                return query;
            }
        }
        
        //todo 这里我应该调用Event里面的Get方法，自己临时写了一个；
         public async Task<Event> EventInformation(string id)
        {
            using (var db = _context)
            {
                var @event = await db.Events.Where(item => item.Id == id).FirstOrDefaultAsync();
                return @event;
            }
        }
    }
}
