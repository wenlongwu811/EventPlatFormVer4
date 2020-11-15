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
    class EventParticipantService
    {
        public EventParticipantService(MvcEpfContext context) // context 配置
        {
            _context = context;
        }

        private static MvcEpfContext _context;

        public async Task<List<EventParticipant>> ToListAllEPs()
        {
            using (var db = _context)
            {
                var eps = await db.EventParticipants.ToListAsync();
                return eps;
            }
        }

        public async Task<EventParticipant> GetEvent(string id) // 根据EPID来查询
        {
            using (var db = _context)
            {
                var ep = await db.EventParticipants.Where(e => e.Id == id).FirstOrDefaultAsync(); // FirstOrDefault方法返回序列中的第一个或者默认值
                return ep;
            }
        }

        public async Task<List<EventParticipant>> GetEventParticipantsAsync(string eventId) // 返回单个Event的所有Participants
        {
            using (var db = _context)
            {
                if (eventId == null)
                    return null;
                var eventParticipants = await db.EventParticipants.Where(ep => ep.Event_Id == eventId).ToListAsync();
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

        public async Task AddEP(EventParticipant ep) // 添加new Event
        {
            try
            {
                using (var db = _context)
                {
                    db.EventParticipants.Add(ep);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                // TODO: 需要根据错误类型返回不同错误信息
                throw new ApplicationException($"添加活动出错{e.Message}");
            }
        }

        public async Task RemoveEP(string epId)
        {
            /**
             * 在Event表中删除该Event
             *
             */
            try
            {
                using (var db = _context)
                {
                    var ep = await db.EventParticipants.Where(ep => ep.Id == epId).FirstOrDefaultAsync();
                    db.EventParticipants.RemoveRange(ep);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                // TODO: 需要根据错误类型返回不同错误信息
                throw new ApplicationException($"删除活动出错{e.Message}");
            }
        }

       

        public async Task<EventParticipant> FindEventParticipantAsync(string epid)
        {
            using (var db = _context)
            {
                var ep = await db.EventParticipants.Where(e => e.Id == epid).FirstOrDefaultAsync();
                return ep;
            }
        }
    }
}
