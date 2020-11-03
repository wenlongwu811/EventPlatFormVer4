using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlatFormVer4.Models
{
    public class Event
    {
        [Key]
        public uint Id { get; set; }

        public uint SponsorId { get; set; }
        [ForeignKey("SponsorId")]

        [Display(Name = "活动名称")]
        [Required(ErrorMessage = "此项必填")]
        public string Name { get; set; }//活

        [Display(Name = "活动等级")]
        [Required(ErrorMessage = "此项必填")]
        public string Rank { get; set; } // 活动性质，

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

        [Display(Name = "举办地址名称")]
        [Required(ErrorMessage = "此项必填")]
        public string Address { get; set; } // 活动举办地址

        [Display(Name = "状态")]
        public int State { get; set; } //状态0/1/2/3表示待提交/审核中/审核失败/审核成功

        //TODO: 确认Detail的类，如果需要上传文件的话应该改成什么类呢？会在后续改成提交文档
        public string Detail { get; set; } // 活动其他细节（报名条件，活动标准，活动具体内容和流程）
        //public Sponsor Sponsor { get; set; } // 主办方
        //public List<Participatant> Participants { get; set; } //参与人员表

        public Event()
        {
            //Todo 待修改id;
            Random random = new Random();
            //Id =(uint)random.Next(100);
            //State = 0;//刚创建de
        }

    }
}
