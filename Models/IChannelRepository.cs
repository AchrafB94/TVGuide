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
    Channel getRandomChannel();
    List<Programme> GetTonightProgrammes(string channelXML);
    string GetCategoryName(int IdCategory);
    string GetPackageName(int IdPackage);
}