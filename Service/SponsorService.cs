using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EventPlatFormVer4.Service
{
    public class SponsorService
    {
        
        private static MvcEpfContext _context;
        public SponsorService(MvcEpfContext context)
        {
            _context = context;
        }

        public async Task<List<Sponsor>> ToListSponsor()//遍历Sponsor表
        {
            using (var db = _context)
            {
                var query =await db.Sponsors.ToListAsync();
                return query;
            }
        }
        public async Task<List<EventParticipant>> ToListEP()//遍历EP表
        {
            // TODO: 将该方法改为sponsor只能查看participant申请自己建立的event 的记录
            using (var db = _context)
            {
                var query =await db.EventParticipants.ToListAsync();
                return query;
            }
        }

        public async Task Add(Sponsor sponsor)//新增赞助者
        {
            using (var db = _context)
            {
                List<Sponsor> sponsors = await ToListSponsor();
                foreach (Sponsor _sponsor in sponsors)
                {
                    if (sponsor.Equals(_sponsor)) throw new ApplicationException("用户已存在，添加失败");
                }
                try
                {
                    db.Sponsors.Add(sponsor);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"{e.Message}");
                }
            }
        }
        public async Task Delete(string id)//删除赞助者
        {
            using (var db = _context)
            {
                var sponsor = await db.Sponsors.Where(item => item.Id == id).FirstOrDefaultAsync();
                db.Sponsors.RemoveRange(sponsor);
                await db.SaveChangesAsync();
            }
        }
        public async Task Update(Sponsor sponsor)//更新赞助者
        {
            using (var db = _context)
            {
                db.Update(sponsor);
               await db.SaveChangesAsync();
            }
        }
        public async Task<Sponsor> FindAsync(string id)//查找赞助者
        {
            using (var db = _context)
            {
                var sponsor = await db.Sponsors.Where(item => item.Id == id).FirstOrDefaultAsync();
                return (Sponsor)sponsor;
            }
        }

        // -----------申请举办event,只要申请了，活动就写入到数据库，event的State=0为待审核
        public async Task Apply(Event _event, string id)
        {
            using (var db = _context)
            {
                var @event = await db.Events.Where(item => item.Id == _event.Id).FirstOrDefaultAsync();
                var sponsor = await db.Sponsors.Where(item => item.Id == id).FirstOrDefaultAsync();
                @event.State = 0;//待审核
                List<EventParticipant> eventParticipants = await ToListEP();
                foreach (EventParticipant eventParticipant in eventParticipants)
                {
                    if (eventParticipant.Equals(@event)) throw new ApplicationException("已经报名过");
                }
                try
                {
                    sponsor.SponEvents.Add(@event);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"{e.Message}");
                }
            }
        }

        // -----------查找已申请的events
        public async Task<List<Event>> ApplyEvents(string id)
        {
            using (var db = _context)
            {
                var sponsor = await db.Sponsors.Where(item => item.Id == id).FirstOrDefaultAsync();
                var events = sponsor.SponEvents;
                return events;
            }
        }

        // -----------向Administor申请取消event,将event的State修改为4
        public async Task Cancel(Event @event,string id)
        {
            using (var db = _context)
            { 
                //var sponsor = await db.Sponsors.Where(item => item.Id == id).FirstOrDefaultAsync();
                var _event = await db.Events.Where(item => (item.Id == @event.Id) && (item.State == 1)).FirstOrDefaultAsync();
                _event.State = 4; //将报名成功的event的PartiState改为4，等待管理员审核
                db.Events.Update(@event);
                db.SaveChanges();
                
            }
        }

        // -----------审核participant报名
        public async Task Accept(EventParticipant EP,string id)//同意未审核,将event的PartiState修改为1
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant = await db.EventParticipants.Where(item => item.Id == EP.Id).FirstOrDefaultAsync();
                eventParticipant.State = 1;
                db.EventParticipants.Update(eventParticipant);
                db.SaveChanges();
            }
        }
        public async Task Deny(EventParticipant EP, string id)//拒绝未审核，将event的PartiState修改为2
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant = await db.EventParticipants.Where(item => item.Id == EP.Id).FirstOrDefaultAsync();
                eventParticipant.State = 2;
                db.EventParticipants.Update(eventParticipant);
                db.SaveChanges();
            }
        }
        public async Task<EventParticipant> Verify(EventParticipant EP, string id)//检查，将所有未审核的participant展示出来
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant =await db.EventParticipants.Where(item => item.Id == EP.Id).FirstOrDefaultAsync();
                return eventParticipant;
            }
        }

        public async Task<EventParticipant> Alter(EventParticipant EP, string id)//修改已审核participant申请表的PartiState
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant = await db.EventParticipants.Where(item => item.Id == EP.Id).FirstOrDefaultAsync();
                return eventParticipant;
            }
        }

        // -----------登记participant成绩
        public async Task Check(EventParticipant EP, string id, string grade)//修改EP表的Grade属性
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == participant.ID);
                EventParticipant eventParticipant = await db.EventParticipants.Where(item => item.Id == EP.Id).FirstOrDefaultAsync();
                eventParticipant.Grade = grade;
                db.EventParticipants.Update(eventParticipant);
                db.SaveChanges();
            }
        }
    }
}
