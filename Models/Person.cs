using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace EventPlatFormVer4.Models
{

    public class Person
    {
        [Key]
        public uint ID { get; set; }
        public uint RoleID { get; set; }//角色TD:0/1/2

        [Display(Name = "姓名")]
        [Required(ErrorMessage ="姓名必填")]
        public string Name { get; set; }//账号

        [Display(Name="邮箱地址")]
        [Required(ErrorMessage = "邮箱必填")]
        public string Email { get; set; }

        [Display(Name="联系方式")]
        [Required(ErrorMessage = "电话必填")]
        public string Phone { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码必填")]
        public string Pwd { get; set; }//密码

        
        public Person()
        {
            Random rm = new Random();
            ID = (uint)rm.Next(100);
            //Todo 用来测试的临时数据100,这个hashtable是一次性的，每次程序重新运行都会更新一次。

            /*Hashtable hashtable = new Hashtable();
            Random rm = new Random();
            int flag = 1;
            for (int i = 0; i < 100&&flag==1; i++)
            {
                uint value = (uint)rm.Next(100);
                if (!hashtable.ContainsValue(value))
                {
                    hashtable.Add(value,value);
                    ID = value;
                    flag = 0;
                }
            }*/
            /* bool flag = true;
             Random random = new Random();
             while (flag)
             {
                 uint value = (uint)random.Next(100);

             }*/
        }

        public Person(Person person)
        {
            this.RoleID = person.RoleID;this.Name = person.Name;this.Email = person.Email;this.Phone = person.Phone;
            this.Pwd = person.Pwd;
        }
    }
}
