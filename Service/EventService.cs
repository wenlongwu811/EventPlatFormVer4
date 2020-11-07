using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlatFormVer4.Service
{
    /**
     * The service class to manage events
     */
    class EventService
    {
        public EventService(MvcEpfContext context) // context 配置
        {
            _context = context;
        }

        private static MvcEpfContext _context;

        public static List<Event> GetAllEvents()
        {
            using (var db = _context)
            {
                return AllEvents(db).ToList();
            }
        }

        public static Event GetEvent(string id) // 根据EventID来查询
        {
            using (var db = _context)
            {
                return AllEvents(db).FirstOrDefault(e => e.Id == id); // FirstOrDefault方法返回序列中的第一个或者默认值
            }
        }

        public static Event AddEvent(Event @event) // 添加new Event
        {
            try
            {
                using (var db = _context)
                {
                    db.Events.Add(@event);
                    db.SaveChanges();
                }
                return @event;
            }
            catch (Exception e)
            {
                // TODO: 需要根据错误类型返回不同错误信息
                throw new ApplicationException($"添加活动出错{e.Message}");
            }
        }

        public static void RemoveEvent(string id)
        {
            try
            {
                using (var db = _context)
                {
                    var @event = db.Events.Include("Participants").Where(e => e.Id == id).FirstOrDefault();
                    db.Events.Remove(@event);
                    db.SaveChanges();
                }
            }
            catch(Exception e)
            {
                // TODO: 需要根据错误类型返回不同错误信息
                throw new ApplicationException($"删除活动出错{e.Message}");
            }
        }

        public static void UpdateEvent(Event newEvent)
        {
            Remo
        }
        public static void RemoveParticipants(string eventId)
        {
            using (var db = _context)
            {
                var oldParticipants = db.Participants.Where(p => p.PartiEvent.Exists(e => e.Id.Equals(eventId))); // 对Participants表查询，返回其中有参与对应活动ID的oldparticipants
                db.Participants.RemoveRange(oldParticipants);

            }
        }
        private static IQueryable<Event> AllEvents(MvcEpfContext db) // 返回所有的 Event
        {
            return db.Events.Include(e => e.Participants.Select(p => p.ID)).Include("Sponsor"); // include 方法会返回和该字段相关的信息
        }
    }
}
