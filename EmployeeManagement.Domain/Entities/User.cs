using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public int? EmployeeId { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public Employee? Employee { get; set; }
    }
}
