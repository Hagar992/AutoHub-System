using System;
using System.Collections.Generic;

namespace AutoHub_System.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Role { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Relationship
        public ICollection<Order> Orders { get; set; }
    }
}
