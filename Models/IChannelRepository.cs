using TVGuide.Models;

public interface IChannelRepository
{
    Channel getChannel(int channelId);
    List<Channel> getAllChannels();
    List<Channel> getChannelsByPackage(string package);
    List<Channel> getChannelsByCategory(string category);
    List<Programme> GetCurrentProgrammes();
    List<Programme> GetProgrammesByChannel(string IdXMLChannel);
    List<Programme> GetProgrammesByNameAndDescription(string query);
}