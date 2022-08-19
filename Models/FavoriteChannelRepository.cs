namespace TVGuide.Models
{
    public class FavoriteChannelRepository : IFavoriteChannelRepository
    {
        private ChannelContext _context;

        public FavoriteChannelRepository(ChannelContext context)
        {
            this._context = context;
        }

        public void DeleteFavoriteChannel(int id)
        {
            throw new NotImplementedException();
        }

        public FavoriteChannel GetFavoriteChannelById(int id)
        {
            throw new NotImplementedException();
        }

        public List<FavoriteChannel> GetFavoriteChannels(string userId)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            return _context.FavoriteChannels.Where(x => x.User == user).ToList();
        }

        public void OrderFavoriteChannel(int position)
        {
            throw new NotImplementedException();
        }

        public FavoriteChannel UpdateFavoriteChannel(int id)
        {
            throw new NotImplementedException();
        }
    }
}
