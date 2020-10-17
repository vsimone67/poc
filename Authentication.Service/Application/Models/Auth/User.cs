using System;
using System.Collections.Generic;

namespace Authentication.Service.Models.Auth
{
    public class User
    {
        public User()
        {
            Roles = new List<string>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public List<string> Roles;

        public Guid UserId { get; set; }
    }
}