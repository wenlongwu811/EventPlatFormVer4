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

        public static List<Sponsor> ToListSponsor()//遍历Sponsor表
        {
            using (var db = _context)
            {
                var query = db.Sponsors.ToList();
                return query;
            }
        }
        public static List<EventParticipant> ToListEP()//遍历EP表
        {
            using (var db = _context)
            {
                var query = db.EventParticipants.ToList();
                return query;
            }
        }

        /*
        public void Add(Sponsor sponsor)//新增赞助者
        {
            try
            {
                using (var db = _context)
                {
                    db.Sponsors.Add(sponsor);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("添加失败");
            }
        }
        */

        public async Task Add(Sponsor sponsor)//新增赞助者
        {
            using (var db = _context)
            {
                List<Sponsor> sponsors = SponsorService.ToListSponsor();
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
                var sponsor = db.Sponsors.Where(item => item.Id == id);
                db.Sponsors.RemoveRange(sponsor);
                db.SaveChanges();
            }
        }
        public async Task Update(Sponsor sponsor)//更新赞助者
        {
            using (var db = _context)
            {
                db.Update(sponsor);
                db.SaveChanges();
            }
        }
        public async Task<Sponsor> Find(string id)//查找赞助者
        {
            using (var db = _context)
            {
                var sponsor = db.Sponsors.Where(item => item.Id == id);
                return (Sponsor)sponsor;
            }
        }

        // -----------申请举办event,只要申请了，活动就写入到数据库，event的State=0为待审核
        public async Task Apply(Event _event, string id)//TODO：需要处理多次申请同名活动产生不同的id问题
        {
            using (var db = _context)
            {
                var @event = (Event)db.Events.Where(item => item.Id == _event.Id);
                var sponsor = (Sponsor)db.Sponsors.Where(item => item.Id == id);
                @event.State = 0;//待审核
                List<EventParticipant> eventParticipants = ToListEP();
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
                //sponsor.SponEvents.Add(@event);
                //db.SaveChanges();
            }

            /*
            using (var db = _context)
            {
                db.Events.Add(@event);
                @event.State = 0;
                db.Events.Update(@event);
                db.SaveChanges();
            }
            */

        }

        // -----------查找已申请的events
        public async Task<List<Event>> ApplyEvents(string id)
        {
            using (var db = _context)
            {
                var sponsor = (Sponsor)db.Sponsors.Where(item => item.Id == id);
                var events = sponsor.SponEvents;
                return events;
                //var query = db.Events.Where(item => item.State == state);
                //return query.ToList();//打印event列表
            }
        }

        // -----------向Administor申请取消event,将event的State修改为4
        public void Cancel(Event @event,string id)
        {
            using (var db = _context)
            { 
                var sponsor = (Sponsor)db.Sponsors.Where(item => item.Id == id);
                var _event = (Event)sponsor.SponEvents.Where(item => (item.Id == @event.Id) && (item.State == 1));
                _event.State = 4; //将报名成功的event的PartiState改为4，等待管理员审核
                db.Events.Update(@event);
                db.SaveChanges();
                /*
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                @event.State = 4;
                db.Events.Update(@event);
                db.SaveChanges();
                */
            }
        }

        // -----------审核participant报名
        public void Accept(EventParticipant EP,string id)//同意未审核,将event的PartiState修改为1
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant = (EventParticipant)db.EventParticipants.Where(item => item.Id == EP.Id);
                eventParticipant.State = 1;
                db.EventParticipants.Update(eventParticipant);
                db.SaveChanges();
            }
        }
        public void Deny(EventParticipant EP, string id)//拒绝未审核，将event的PartiState修改为2
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant = (EventParticipant)db.EventParticipants.Where(item => item.Id == EP.Id);
                eventParticipant.State = 2;
                db.EventParticipants.Update(eventParticipant);
                db.SaveChanges();
            }
        }
        public EventParticipant Verify(EventParticipant EP, string id)//检查，将所有未审核的participant展示出来
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant = db.EventParticipants.Where(item => item.Id == EP.Id).FirstOrDefault();
                return eventParticipant;
            }
        }

        public EventParticipant Alter(EventParticipant EP, string id)//修改已审核participant申请表的PartiState
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == id);
                EventParticipant eventParticipant = (EventParticipant)db.EventParticipants.Where(item => item.Id == EP.Id);
                return eventParticipant;
            }
        }

        // -----------登记participant成绩
        public void Check(EventParticipant EP, string id, string grade)//修改EP表的Grade属性
        {
            using (var db = _context)
            {
                //Event @event = (Event)db.Events.Where(item => item.Id == participant.ID);
                EventParticipant eventParticipant = (EventParticipant)db.EventParticipants.Where(item => item.Id == EP.Id);
                eventParticipant.Grade = grade;
                db.EventParticipants.Update(eventParticipant);
                db.SaveChanges();
            }
        }
    }
}
