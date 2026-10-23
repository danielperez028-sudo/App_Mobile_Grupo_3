using System;
using System.Collections.Generic;
using System.Text;

namespace ReadBuddy.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Skill { get; set; } = string.Empty; 
        public string CorrectAnswer { get; set; } = string.Empty;

        
        public string StoryId { get; set; } = string.Empty; 
        public string StoryTitle { get; set; } = string.Empty; 
        public string Options { get; set; } = string.Empty;
    }
}
