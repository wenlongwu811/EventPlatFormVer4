using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatFormVer4.Models
{
    public class Event : IComparable<Event>
    {
        [Key]
        public string Id { get; set; }

        public string SponsorId { get; set; }
        [ForeignKey("SponsorId")]
        public Sponsor Sponsor { get; set; }
        public string SponsorName { get => (Sponsor != null) ? Sponsor.Name : ""; }

        public List<Participant> Participants { get; set; } //参与人员表

        [Display(Name = "活动名称")]
        [Required(ErrorMessage = "此项必填")]
        public string Name { get; set; }

        [Display(Name = "活动等级")]
        [Required(ErrorMessage = "此项必填")]
        public string Rank { get; set; } // 活动性质，

        public DateTime CreateTime { get; set; } // 活动提交申请时间

        [Display(Name = "活动开始时间")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "此项必填")]
        public DateTime EventStartTime { get; set; } // 活动开始时间

        [Display(Name = "活动结束名称")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "此项必填")]
        public DateTime EventEndTime { get; set; } // 活动结束时间

        [Display(Name = "报名开始名称")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "此项必填")]
        public DateTime SignUpStartTime { get; set; } // 报名开始时间

        [Display(Name = "报名截止名称")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "此项必填")]
        public DateTime SignUpEndTime { get; set; } // 报名结束时间

        [Display(Name = "举办地址")]
        [Required(ErrorMessage = "此项必填")]
        public string Address { get; set; } // 活动举办地址

        [Display(Name = "状态")]

        public int State { get; set; } //赛事申请状态，0/1/2/3/4表示待审核/审核成功/审核失败/取消/申请取消

        //TODO: 确认Detail的类，如果需要上传文件的话应该改成什么类呢？会在后续改成提交文档
        public string Detail { get; set; } // 活动其他细节（报名条件，活动标准，活动具体内容和流程)


        public Event()
        {
            Id = Guid.NewGuid().ToString(); // to generate a new id
            Participants = new List<Participant>();
            CreateTime = DateTime.Now;
        }

        public Event(Sponsor sponsor,List<Participant> participants) : this()
        {
            this.Sponsor = sponsor;
            this.CreateTime = DateTime.Now;
            if (participants != null) Participants = participants;
        }

        public void AddParticipant(Participant participant)
        {
            if(Participants.Contains(participant))
                throw new ApplicationException($"添加错误：参与者已存在！");
            Participants.Add(participant);
        }

        public void RemoveParticipant(Participant participant)
        {
            Participants.Remove(participant);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append($"Id:{Id}, name:{Name}, sponsor:{Sponsor},createTime:{CreateTime}");
            Participants.ForEach(participant => strBuilder.Append("\n\t" + participant));
            return strBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            var e = obj as Event;
            return e != null &&
                Id == e.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -294571041;
            hashCode = hashCode * -1561131293 + Id.GetHashCode();
            hashCode = hashCode * -1561131293 + EqualityComparer<string>.Default.GetHashCode(SponsorName);
            hashCode = hashCode * -1561131293 + CreateTime.GetHashCode();
            return hashCode;
        }
        int IComparable<Event>.CompareTo(Event other)
        {
            if (other == null) return 1;
            return this.Id.CompareTo(other.Id);
        }
    }
}
