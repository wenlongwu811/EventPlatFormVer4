using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Razor.Language;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace EventPlatFormVer4.Service
{
    public class ParticipantService
    {
        private static MvcEpfContext _context;
        public ParticipantService(MvcEpfContext context)
        {
            _context = context;
        }
        public static List<Participant> ToListParticipant()//遍历Participant表
        {
            using(var db = _context)
            {
                var query = db.Participants.ToList();
                return query;
            }
        }
        public static List<EventParticipant> ToListEP()//遍历EP表
        {
            using(var db = _context)
            {
                var query = db.EventParticipants.ToList();
                return query;
            }
        }
        //增加参赛者
        public async Task Add(Participant participant)
        {
            using (var db = _context)
            {
                List<Participant> participants =ParticipantService.ToListParticipant();
                foreach(Participant _participant in participants)
                {
                    if (participant.Equals(_participant)) throw new ApplicationException("用户已存在，添加失败");
                }
                    db.Participants.Add(participant);
                    await db.SaveChangesAsync();
            }
        }
        //删除参赛者
        public async Task Delete(string id)
        {
            using (var db = _context)
            {
                var participant = db.Participants.Where(item => item.ID == id);
                db.Participants.RemoveRange(participant);
                await db.SaveChangesAsync();
            }
        }
        //更新参赛者信息
        public async Task Update(Participant participant)
        {
            using (var db = _context)
            {
                db.Update(participant);
                await db.SaveChangesAsync();
            }
        }
        //查找参赛者
        public async Task<Participant> Find(string id)
        {
            using (var db = _context)
            {
                var participant = await db.Participants.Where(item => item.ID == id).FirstOrDefaultAsync();
                return participant;
            }
        }

        //报名，将对应的event添加到自己的List里面，并将EP的State改为0
        public async Task Apply(EventParticipant EP,string id)
        {
            using (var db = _context)
            {
                var @event = (EventParticipant)db.Events.Where(item => item.Id == EP.Id);
                var participant = (Participant)db.Participants.Where(item => item.ID == id);
                @event.Participant = participant;
                @event.State = 0;
                List<EventParticipant> eventParticipants = ToListEP();
                foreach(EventParticipant eventParticipant in eventParticipants)
                {
                    if (eventParticipant.Equals(@event)) throw new ApplicationException("已经报名过");
                }
                try
                {
                    participant.PartiEvent.Add(@event);
                    await db.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    throw new ApplicationException($"{e.Message}");
                }
            }
        }
        //查找已参加的比赛
        public async Task<List<EventParticipant>> FindEvent(string id)
        {
            using (var db=_context)
            {
                return await db.EventParticipants.Where(item => item.ParticipantId == id).ToListAsync();
            }
        }
        //退赛，将List中已经报名成功的event的PartiState改为3
        public async Task ExistEvent(EventParticipant EP,string id)
        {
            using (var db=_context)
            {
                var participant = (Participant)db.Participants.Where(item => item.ID == id);
                var eventParticipant = (EventParticipant)db.EventParticipants.Where(item => (item.Event_Id == EP.Id)&&(item.ParticipantId==id)&&(item.State==1));

                eventParticipant.State = 3;
                await db.SaveChangesAsync();
            }
        }



    }
}
