using System;
using System.Collections.Generic;
using System.Text;

namespace ReadBuddy.Models
{
    public class VocabularyWord
    {
        public int Id { get; set; }

        public string Term { get; set; } = string.Empty;

        public string Definition { get; set; } = string.Empty;

        public string Example { get; set; } = string.Empty;      



        public string ExampleLabel => string.IsNullOrWhiteSpace(Example)

            ? "Sin ejemplo"

            : $"Ej: {Example}";
    }
}
