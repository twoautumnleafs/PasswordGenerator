using System;

namespace PasswordGeneratorApp.Models
{
    public class PasswordLogEntry
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}