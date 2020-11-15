using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<Event>> GetAllEvents()
        {
            using (var db = _context)
            {
                var events = await db.Events.ToListAsync();
                return events;
            }
        }

        public async Task<Event> GetEvent(string id) // 根据EventID来查询
        {
            using (var db = _context)
            {
                var @event = await db.Events.Where(e => e.Id == id).FirstOrDefaultAsync(); // FirstOrDefault方法返回序列中的第一个或者默认值
                return @event; 
            }
        }

        public async Task<List<EventParticipant>> GetEventParticipantsAsync(string eventId) // 返回单个Event的所有Participants
        {
            using (var db = _context)
            {
                if (eventId == null)
                    return null;
                var eventParticipants = await db.EventParticipants.Where(ep => ep.EventId == eventId).ToListAsync();
                return eventParticipants;
            }
        }

        public async Task<List<EventParticipant>> GetParticipantEventsAsync(string participantId) // 返回单个Participant的所有Events
        {
            using (var db = _context)
            {
                if (participantId == null)
                {
                    return null;
                }
                var participantEvents = await db.EventParticipants.Where(ep => ep.ParticipantId == participantId).ToListAsync();
                return participantEvents;
            }
        }

        public async Task AddEvent(Event @event) // 添加new Event
        {
           
                using (var db = _context)
                {
                    db.Events.Add(@event);
                    await db.SaveChangesAsync();
                }
            
            
        }

        public async Task RemoveEvent(string id)
        {
            /**
             * 在Event表中删除该Event
             *
             */
            try
            {
                using (var db = _context)
                {
                    var @event = await db.Events.Where(e => e.Id == id).FirstOrDefaultAsync();
                    db.Events.RemoveRange(@event);
                    await db.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                // TODO: 需要根据错误类型返回不同错误信息
                throw new ApplicationException($"删除活动出错{e.Message}");
            }
        }

        public async Task UpdateEventParticipants(Event newEvent)
        {
            /**
             * 更新E-P表
             */
            await RemoveEventParticipants(newEvent.Id); // 先在E-P表中删除之前的参赛记录
            try
            {
                using (var db = _context)
                {
                    db.Entry(newEvent).State = EntityState.Modified;
                    await db.EventParticipants.AddRangeAsync(newEvent.EventParticipants); // 在E-P表中添加新的参赛记录
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                // TODO: 需要根据错误类型返回不同错误信息
                throw new ApplicationException($"添加E-P表中记录时出错{e.Message}");
            }
        }
        public async Task RemoveEventParticipants(string eventId)
        {
            /**
             * 在E-P表中删除参加了此event的所有参赛记录
             */
            try {
                using (var db = _context)
                {
                    var oldParticipants = await db.EventParticipants.Where(p => p.EventId == eventId).ToListAsync(); // 对E-P表查询，EventID相等的oldParticipants
                    // TODO: how to apply async method in ?
                    db.EventParticipants.RemoveRange(oldParticipants); 
                    await db.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                // TODO: 需要根据错误类型返回不同错误信息
                throw new ApplicationException($"删除E-P表中记录时出错{e.Message}");
            }
        }

        public async Task<Event> FindEventAsync(string id)
        {
            using (var db = _context)
            {
                var @event = await db.Events.Where(e => e.Id == id).FirstOrDefaultAsync();
                return @event;
            }
        }
    }
}
