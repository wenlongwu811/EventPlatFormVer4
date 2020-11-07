using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EventPlatFormVer4.Models
{
    public class EventParticipant
    {
        [Key]
        public string Id { get; set; }
        public int Index { get; set; }
        public Participant Participant { get; set; }
        public string ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public string ParticipantName { get => Participant != null ? Participant.Name : ""; }
        public string Grade { get; set; }
        public Event Event { get; set; }
        public string EventId { get; set; }
        [ForeignKey("EventId")]
        public string EventName { get => Event != null ? Event.Name : "" ; }
        // TODO: decide the State number
        public int State { get; set; }//赛事报名状态，0/1/2/3表示待审核/报名成功/报名失败/取消

        public EventParticipant() {
            Id = Guid.NewGuid().ToString();
        }

        public EventParticipant(int index,  Event @event, Participant participant)
        {
            this.Index = index;
            this.Event = @event;
            this.Participant = participant;
        }

        public override string ToString()
        {
            return $"[No.:{Index},event:{EventName},participant:{ParticipantName},grade:{Grade},state:{State}]";
        }

        public override bool Equals(object obj)
        {
            var item = obj as EventParticipant;
            return item != null &&
                EventName == item.EventName &&
                ParticipantName == item.ParticipantName;
        }

        public override int GetHashCode()
        {
            var hashCode = -231495720;
            hashCode = hashCode * -1525538493 + Index.GetHashCode();
            hashCode = hashCode * -1525538493 + EqualityComparer<string>.Default.GetHashCode(ParticipantName);
            hashCode = hashCode * -1525538493 + EqualityComparer<string>.Default.GetHashCode(EventName);
            return hashCode;
        }
    }
}
