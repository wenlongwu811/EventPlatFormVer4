using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EventPlatFormVer4.Models
{
    public class Sponsor
    {
        [Key]
        public string Id { get; set; }//唯一标识的Id
        public uint RoleID { get; set; }//赞助者角色Id为1

        [ForeignKey("EventId")]
        public List<Event> SponEvents { get; set; }//赞助的活动列表

        //[Display(Name = "请输入认证机构")]
        public string Certificate { get; set; } // 机构认证, （院级，校级，省级；武大办，华科办，北大办……）
        
        //[Display(Name = "姓名")]
        //[Required(ErrorMessage = "姓名必填")]
        public string Name { get; set; }//账号

        //[Display(Name = "邮箱地址")]
        //[Required(ErrorMessage = "邮箱必填")]
        public string Email { get; set; }//邮件

        //[Display(Name = "联系方式")]
        //[Required(ErrorMessage = "电话必填")]
        public string Phone { get; set; }//电话

        //[Display(Name = "密码")]
        //[Required(ErrorMessage = "密码必填")]
        public string Pwd { get; set; }//密码

        public Sponsor()
        {
            Id = Guid.NewGuid().ToString();//Id唯一性
            RoleID = 1;
            SponEvents = new List<Event>();
        }
        public Sponsor(List<Event> events) : this()
        {
            if (events != null) SponEvents = events;
        }
        public override bool Equals(object obj)
        {
            var sponsor = obj as Sponsor;
            return sponsor != null && Name == sponsor.Name && Email == sponsor.Email && Phone == sponsor.Phone;
        }
        public override int GetHashCode()
        {
            /*
            var hashCode = -2127770830;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            return hashCode;
            */
            return -2127770830 + Id.GetHashCode() + RoleID.GetHashCode();
        }
        public override string ToString()//提取赞助者信息
        {
            return "姓名：" + Name + "\n" + "邮箱地址：" + Email + "\n" + "电话号码：" + Phone + "\n";
        }
    }
}
