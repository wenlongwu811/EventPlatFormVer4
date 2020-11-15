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

        public Participant Participant { get; set; }

        [ForeignKey("ParticipantId")]
        public string ParticipantId { get; set; }

        [Display(Name = "成绩")]
        public string Grade { get; set; }

        public Event Event { get; set; }

        [ForeignKey("Event_Id")]
        public string Event_Id { get; set; }

        public int State { get; set; }

        public EventParticipant() {
            Id = Guid.NewGuid().ToString();
        }

        public EventParticipant(Event @event, Participant participant)
        {
            this.Event = @event;
            this.Participant = participant;
        }

        public override string ToString()
        {
            return $"[No.:{Id},event:{Event.Name},participant:{Participant.Name},grade:{Grade},state:{State}]";
        }

        public override bool Equals(object obj)
        {
            var item = obj as EventParticipant;
            return item != null &&
                Event.Id == item.Event.Id &&
                Participant.ID == item.Participant.ID;
        }

        public override int GetHashCode()
        {
            var hashCode = -231495720;
            hashCode = hashCode * -1525538493 + Id.GetHashCode();
            hashCode = hashCode * -1525538493 + EqualityComparer<string>.Default.GetHashCode(Participant.Name);
            hashCode = hashCode * -1525538493 + EqualityComparer<string>.Default.GetHashCode(Event.Name);
            return hashCode;
        }
    }
}
