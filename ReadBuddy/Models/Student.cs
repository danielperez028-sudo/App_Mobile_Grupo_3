using System;
using System.Collections.Generic;
using System.Text;

namespace ReadBuddy.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Avatar { get; set; } = string.Empty; 
        public string UserName { get; set; } = string.Empty;
    }
}
