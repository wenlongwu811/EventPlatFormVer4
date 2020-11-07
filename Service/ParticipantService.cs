﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Razor.Language;

namespace EventPlatFormVer4.Service
{
    public class ParticipantService
    {
        private static MvcEpfContext _context;
        public ParticipantService(MvcEpfContext context)
        {
            _context = context;
        }
        public static List<Participant> ToList()//遍历数据库
        {
            using(var db = _context)
            {
                var query = db.Participants.ToList();
                return query;
            }
        }
        //增加参赛者
        public void Add(Participant participant)
        {
            using (var db = _context)
            {
                List<Participant> participants =ParticipantService.ToList();
                foreach(Participant _participant in participants)
                {
                    if (participant.Equals(_participant)) throw new ApplicationException("用户已存在，添加失败");
                }
                try
                {
                    db.Participants.Add(participant);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("用户已存在，添加失败");
                }
            }
        }
        //删除参赛者
        public void Delete(string id)
        {
            using (var db = _context)
            {
                var participant = db.Participants.Where(item => item.ID == id);
                db.Participants.RemoveRange(participant);
                db.SaveChanges();
            }
        }
        //更新参赛者信息
        public void Update(Participant participant)
        {
            using (var db = _context)
            {
                db.Update(participant);
                db.SaveChanges();
            }
        }
        //查找参赛者
        public Participant Find(string id)
        {
            using (var db = _context)
            {
                var participant = db.Participants.Where(item => item.ID == id);
                return (Participant)participant;
            }
        }
        //报名，将对应的event添加到自己的List里面，并将EP的State改为0
        public void Apply(EventParticipant EP,string id)
        {
            using (var db = _context)
            {
                var @event = (EventParticipant)db.Events.Where(item => item.Id == EP.Id);
                var participant = (Participant)db.Participants.Where(item => item.ID == id);
                @event.Participant = participant;
                @event.State = 0;
                participant.PartiEvent.Add(@event);
                db.SaveChanges();
            }
        }
        //查找已参加的比赛
        public List<EventParticipant> FindEvent(string id)
        {
            using (var db=_context)
            {
                var participant = (Participant)db.Participants.Where(item => item.ID == id);
                return participant.PartiEvent;
            }
        }
        //退赛，将List中已经报名成功的event的PartiState改为3
        public void ExitEvent(EventParticipant EP,string id)
        {
            using (var db=_context)
            {
                var participant = (Participant)db.Participants.Where(item => item.ID == id);
                var eventParticipant = (EventParticipant)db.EventParticipants.Where(item => (item.EventId == EP.Id)&&(item.ParticipantId==id)&&(item.State==1));
                eventParticipant.State = 3;
            }
        }



    }
}
