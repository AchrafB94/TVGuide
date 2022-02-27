namespace TVGuide.Models
{
    public interface IChannelRepository
    {
        Channel getChannel(string channelId);
        IEnumerable<Channel> getChannels();
        List<Program> getPrograms(string channelId);
        Program getProgram(string programTitle);
    }
}
