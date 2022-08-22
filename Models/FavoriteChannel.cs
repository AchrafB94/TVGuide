

using System.ComponentModel.DataAnnotations;

namespace TVGuide.Models
{
    public class FavoriteChannel
    {
        [Key] public int Id { get; set; }
        public int FavoritePosition { get; set; }
        [Required] public TVGuideUser User { get; set; }
        [Required] public Channel Channel { get; set; }
    }
}
