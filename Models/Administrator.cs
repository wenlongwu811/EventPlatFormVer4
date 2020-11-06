using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlatFormVer4.Models
{
    public class Administrator
    {
        [Key]
        public string Id { get; set; }
        public string RoleID { get; set; }//角色iD:0/1/2

        [Required]
        [RegularExpression(@"[0-9]{13}", ErrorMessage = "13位数字")]
        public string Name { get; set; }


        [RegularExpression(@"^\w+@[A-Za-z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "请输入正确的格式")]
        [Required]
        public string Email { get; set; }


        [RegularExpression(@"[A-Za-z0-9]{6,}", ErrorMessage = "至少6位数字字母组合")]
        [Required]
        public string Pwd { get; set; }

        public Administrator()
        {
            Id = Guid.NewGuid().ToString();
            RoleID = "0";
        }

        public Administrator(string name, string email, string pwd) : this()
        {
            Name = name; Email = email; Pwd = pwd;
        }

        public override bool Equals(object obj)
        {
            return obj is Administrator administrator &&
                   Id == administrator.Id &&
                   RoleID == administrator.RoleID &&
                   Name == administrator.Name &&
                   Email == administrator.Email &&
                   Pwd == administrator.Pwd;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, RoleID, Name, Email, Pwd);
        }

    }
}
