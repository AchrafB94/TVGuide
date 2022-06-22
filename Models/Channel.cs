using System.ComponentModel.DataAnnotations;

namespace TVGuide.Models
{
    public class Channel
    {
        [Key] public int Id { get; set; }
        public int Position { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Logo { get; set; } = string.Empty;
        public string? IdXML { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public string? Package { get; set; } = string.Empty;
    }
}
