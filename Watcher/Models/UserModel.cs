﻿namespace Watcher.Models
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

        public string Theme { get; set; }

        public string Lang { get; set; }
    }
}
