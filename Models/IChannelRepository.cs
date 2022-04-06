using TVGuide.Models;

public interface IChannelRepository
{
    Channel getChannel(int channelId);
    List<Channel> getAllChannels();
    List<Channel> getChannelsByPackage(string package);
    List<Channel> getChannelsByCategory(string category);
    List<Channel> getChannelsByPositions(int start, int end);
    List<Programme> GetProgrammesByChannel(string IdXMLChannel, string XML);
    Programme GetCurrentProgram(int IdXMLChannel);
    List<Programme> GetProgrammesByNameAndDescription(string query);
}