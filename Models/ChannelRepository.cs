

using System.Xml.Linq;

namespace TVGuide.Models
{
    public class ChannelRepository : IChannelRepository
    {
        XDocument document;
        public ChannelRepository()
        {
            string uri = "https://iptv-org.github.io/epg/guides/eg-ar/elcinema.com.epg.xml";
            this.document = XDocument.Load(uri);
        }

        public Channel getChannel(string channelId)
        {
            XElement xeChannel = document.Root.Elements("channel").FirstOrDefault(channel => channel.Attribute("id").Value == channelId);
            Channel channel = new Channel(
               channelId,
               xeChannel.Element("display-name").Value,
               xeChannel.Element("url").Value,
               xeChannel.Element("icon").Attribute("src").Value);
            return channel;
        }

        public IEnumerable<Channel> getChannels()
        {
            throw new NotImplementedException();
        }

        public Program getProgram(string programTitle)
        {
            throw new NotImplementedException();
        }

        public List<Program> getPrograms(string channelId)
        {
            IEnumerable<XElement> xePrograms = document.Root.Elements("programme").Where(program => program.Attribute("channel").Value == channelId);
            List<Program> programs = new List<Program>();
            string start = string.Empty, stop = string.Empty, title = string.Empty, desc = string.Empty, category = string.Empty, image = string.Empty;

            foreach (XElement xeProgram in xePrograms)
            {
                start = xeProgram.Attribute("start") != null ? xeProgram.Attribute("start").Value : string.Empty;
                start = start.Substring(8, 2) + ":" + start.Substring(10, 2);
                stop = xeProgram.Attribute("stop") != null ? xeProgram.Attribute("stop").Value : string.Empty;
                stop = stop.Substring(8, 2) + ":" + stop.Substring(10, 2);

                title = xeProgram.Element("title") != null ? xeProgram.Element("title").Value : string.Empty;
                desc = xeProgram.Element("desc") != null ? xeProgram.Element("desc").Value : string.Empty;
                category = xeProgram.Element("category") != null ? xeProgram.Element("category").Value : string.Empty;
                image = xeProgram.Element("icon").Attribute("src") != null ? xeProgram.Element("icon").Attribute("src").Value : string.Empty;

                Program program = new Program(start, stop, title, desc, category, image);

                programs.Add(program);
            }
            return programs;
        }
    }
}
