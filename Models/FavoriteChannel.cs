

namespace TVGuide.Models
{
    public class FavoriteChannel
    {
        public int Id { get; set; }
        public int FavoritePosition { get; set; }
        public TVGuideUser User { get; set; }

        public Channel Channel { get; set; }
    }
}
