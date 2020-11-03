using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlatFormVer4.Models
{
    public class Sponsor
    {
        public uint Id { get; set; }
        public uint RoleID { get; set; }//角色TD:0/1/2

        public List<Event> events { get; set; }
        [Display(Name = "请输入认证机构")]
        public string Certificate { get; set; } // 机构认证, （院级，校级，省级；武大办，华科办，北大办……）
        
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名必填")]
        public string Name { get; set; }//账号

        [Display(Name = "邮箱地址")]
        [Required(ErrorMessage = "邮箱必填")]
        public string Email { get; set; }

        [Display(Name = "联系方式")]
        [Required(ErrorMessage = "电话必填")]
        public string Phone { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码必填")]
        public string Pwd { get; set; }//密码

        public Sponsor()
        {
            Random rm = new Random();
            //Todo  id要唯一
            //啦啦啦测试一下
            //ygx test
            Id = (uint)rm.Next(100);
            RoleID = 1;
        }


    }
}
