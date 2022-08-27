namespace TVGuide.Models
{
    public class UserFavoritesViewModel
    {
        public List<Programme> userProgrammes {get; set;}
        public List<FavoriteChannel> userFavoriteChannels {get; set;}
        public List<Programme> favoriteChannelsCurrentProgrammes { get; set;}

        public UserFavoritesViewModel()
        {
            this.userFavoriteChannels = new List<FavoriteChannel>();
            this.favoriteChannelsCurrentProgrammes = new List<Programme>();
            this.userProgrammes = new List<Programme>();
        }
    }
}
