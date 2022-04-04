

using System.Xml.Linq;

namespace TVGuide.Models
{
    public class ChannelRepository : IChannelRepository
    {
        public ChannelRepository()
        {
        }


        List<Channel> IChannelRepository.getAllChannels()
        {
            throw new NotImplementedException();
        }

        Channel IChannelRepository.getChannel(string channelId)
        {
            throw new NotImplementedException();
        }

        List<Channel> IChannelRepository.getChannelsByCategory(string category)
        {
            throw new NotImplementedException();
        }

        List<Channel> IChannelRepository.getChannelsByPackage(string package)
        {
            throw new NotImplementedException();
        }

        List<Channel> IChannelRepository.getChannelsByPositions(int start, int end)
        {
            throw new NotImplementedException();
        }
    }
}
