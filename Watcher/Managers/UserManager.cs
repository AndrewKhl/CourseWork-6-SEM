using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watcher.Models;

namespace Watcher
{
    public class UserManager : DbContext
    {
        public UserManager() : base("DBMonitoringTool")
        {
            if (Users.Count() == 0)
            {
                Users.Add(new UserModel()
                {
                    Name = "Admin",
                    Password = "123456",
                    IsAdmin = true,
                    IP = "127.0.0.1",
                    Port = 9000,

                });

                Users.Add(new UserModel()
                {
                    Name = "Red",
                    Password = "123456",
                    IsAdmin = false,
                    IP = "127.0.0.1",
                    Port = 9000,
                });

                SaveChanges();
            }
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
