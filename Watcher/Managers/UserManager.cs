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
        public DbSet<UserModel> Users { get; set; }

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
                    Name = "Andrew",
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
        
        public UserModel GetUser(string name, string password)
        {
            return Users.Where(u => u.Name == name && u.Password == password).FirstOrDefault();
        }

        public bool HaveUser(string name)
        {
            return Users.Where(u => u.Name == name).Count() != 0;
        }

        public void AddUser(string name, string password)
        {
            Users.Add(new UserModel()
            {
                Name = name,
                Password = password,
                IsAdmin = false,
            });

            SaveChanges();
        }

        public void UpdateUser(UserModel user)
        {
            SaveChanges();
        }
    }
}
