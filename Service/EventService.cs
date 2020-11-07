using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlatFormVer4.Service
{
    /**
     * The service class to manage events
     */
    public class EventService
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
            /**
             * 删除Event
             *
             */
            try
            {
                using (var db = _context)
                {
                    var @event = db.Events.Include("EventParticipants").Where(e => e.Id == id).FirstOrDefault();
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

        public static void UpdateEventParticipant(Event newEvent)
        {
            /**
             * 更新E-P表
             */
            RemoveParticipants(newEvent.Id); // 先在E-P表中删除之前的参赛记录
            using (var db = _context)
            {
                db.Entry(newEvent).State = EntityState.Modified;
                db.EventParticipants.AddRange(newEvent.EventParticipants); // 在E-P表中添加新的参赛记录
                db.SaveChanges();
            }
        }
        public static void RemoveParticipants(string eventId)
        {
            /**
             * 在E-P表中删除参加了此event的所有参赛记录
             */
            using (var db = _context)
            {
                var oldParticipants = db.EventParticipants.Where(p => p.EventId == eventId); // 对E-P表查询，EventID相等的oldParticipants
                db.EventParticipants.RemoveRange(oldParticipants);

            }
        }
        private static IQueryable<Event> AllEvents(MvcEpfContext db) // 返回所有的 Event
        {
            return db.Events.Include(e => e.EventParticipants.Select(p => p.Participant)).Include("Sponsor"); // include 方法会返回和该字段相关的信息
        }

        public static void Export(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Event>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xs.Serialize(fs, GetAllEvents());
            }
        }
        public static void Import(String path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Event>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                List<Event> temp = (List<Event>)xs.Deserialize(fs);
                temp.ForEach(@event =>
                {
                    try
                    {
                        AddEvent(@event);
                    }
                    catch(Exception e)
                    {
                        throw new ApplicationException($"导入活动出错{e.Message}");
                    }
                });
            }
        }
    }
}
