namespace TVGuide.Models
{
    public interface IChannelRepository
    {
        Channel getChannel(string channelId);
        List<Channel> getAllChannels();
        List<Channel> getChannelsByPackage(string package);
        List<Channel> getChannelsByCategory(string category);
        List<Channel> getChannelsByPositions(int start, int end);
    }
}
