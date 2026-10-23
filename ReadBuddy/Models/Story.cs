using System;
using System.Collections.Generic;
using System.Text;

namespace ReadBuddy.Models
{
    public class Story

    {

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;    

        public string Skill { get; set; } = string.Empty;      

        public string Category { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Cover { get; set; } = "📘";

        public bool IsActive { get; set; } = true;




        public string StatusLabel => IsActive ? "Activa" : "Oculta";

    }
}
