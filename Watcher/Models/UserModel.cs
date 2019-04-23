using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public bool UseProcess { get; set; }

        public int Theme { get; set; }

        public string Lang { get; set; }
    }
}
