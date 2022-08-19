namespace TVGuide.Models
{
    public interface IFavoriteChannelRepository
    {
        List<FavoriteChannel> GetFavoriteChannels(string userId);
        FavoriteChannel UpdateFavoriteChannel(int id);
        void DeleteFavoriteChannel(int id);
        FavoriteChannel GetFavoriteChannelById(int id);
        void OrderFavoriteChannel(int position);
    }
}