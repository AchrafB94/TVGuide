using TVGuide.Models;

public interface IChannelRepository
{
    Channel getChannel(int channelId);
    List<Channel> getAllChannels();
    List<Channel> getChannelsByPackage(int IdPackage);
    List<Channel> getChannelsByCategory(int IdCategory);
    Task<List<Programme>> GetCurrentProgrammes();
    Task<List<Programme>> GetCurrentProgrammes(int IdCategory);
    List<IGrouping<DayOfWeek,Programme>> GetProgrammesByChannel(string IdXMLChannel);
    List<Programme> GetProgrammesByNameAndDescription(string query);
    TonightViewModel GetTonightProgrammes();
    string GetCategoryName(int IdCategory);
    string GetPackageName(int IdPackage);
    List<Programme> GetUserProgrammes(string keywords);
    List<Category> GetCategories();
    List<FavoriteChannel> GetUserFavoriteChannels(TVGuideUser user);

    List<Programme> GetCurrentProgrammesByChannel(string idXMLChannel);
    void AddFavoriteChannel(TVGuideUser user, int IdChannel);
}