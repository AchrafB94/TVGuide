using System.ComponentModel.DataAnnotations;

namespace TVGuide.Models
{
    public class Channel
    {
        [Key] public int Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string? IdXML { get; set; }
        public string Category { get; set; }
        public string? Package { get; set; }
        public string? XML { get; set; }
    }
}
