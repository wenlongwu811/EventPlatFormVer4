using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EventPlatFormVer4.Service
{
    public class AdministratorService
    {
        private static MvcEpfContext _context;
        public AdministratorService(MvcEpfContext context)
        {
            _context = context;
        }

        public static void Add(Administrator administrator)
        {
            try
            {
                using (var db =_context)
                {
                    db.Administrators.Add(administrator);
                    db.SaveChanges();
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public static void Delete(string id)
        {

        }

        public static void Find(string id)
        {

        }

         public static void Update(Administrator administrator)
        {

        }

        public static void Accept()
        {

        }

        public static void Deny()
        {

        }

        public static void Verify()
        {

        }

        public static void Alter()
        {

        }

    }
}
