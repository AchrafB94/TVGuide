

using System.Xml.Linq;

namespace TVGuide.Models
{
    public class ChannelRepository : IChannelRepository
    {
        public IEnumerable<XElement> channelData;
        public IEnumerable<XElement> programData;
        public ChannelRepository()
        {
            XDocument xdArabic = XDocument.Load("https://iptv-org.github.io/epg/guides/eg-ar/elcinema.com.epg.xml");
            XDocument xdBein = XDocument.Load("https://iptv-org.github.io/epg/guides/qa/beinsports.com.epg.xml");
            XDocument xdOSN = XDocument.Load("https://iptv-org.github.io/epg/guides/ae-ar/osn.com.epg.xml");
            //XDocument xdSky = XDocument.Load("https://iptv-org.github.io/epg/guides/de/sky.de.epg.xml");
            //XDocument xdHDTV = XDocument.Load("https://iptv-org.github.io/epg/guides/de/hd-plus.de.epg.xml");
            XDocument xdCanal = XDocument.Load("https://iptv-org.github.io/epg/guides/fr/programme-tv.net.epg.xml");
            //XDocument xdMovistar = XDocument.Load("https://iptv-org.github.io/epg/guides/es/gatotv.com.epg.xml");
            //XDocument xdPolsat = XDocument.Load("https://iptv-org.github.io/epg/guides/pl/programtv.onet.pl.epg.xml");
            //XDocument xdMediaset = XDocument.Load("https://iptv-org.github.io/epg/guides/it/mediaset.it.epg.xml");

            this.channelData = xdArabic.Root.Elements("channel").Union(xdOSN.Root.Elements("channel")).Union(xdBein.Root.Elements("channel")).Union(xdCanal.Root.Elements("channel"));
            var duplicates = channelData.GroupBy(g => (string)g.Attribute("id"))
            .Where(g => g.Count() > 1).SelectMany(g => g.Take(1));

            duplicates.Remove();

            this.programData = xdArabic.Root.Elements("programme").Where(program => program.Attribute("start").Value.StartsWith("20220303"));
        }

        public Channel getChannel(string channelId)
        {
            XElement xeChannel = channelData.FirstOrDefault(channel => channel.Attribute("id").Value == channelId);
            Channel channel = new Channel(
               channelId,
               xeChannel.Element("display-name").Value,
               xeChannel.Element("icon").Attribute("src").Value); 
            return channel;
        }

        public List<Channel> getChannels()
        {
            List<Channel> channels = new List<Channel>();
            

            foreach (XElement xeChannel in this.channelData)
            {
                Channel channel = new Channel(
                    xeChannel.Attribute("id").Value,
                    xeChannel.Element("display-name").Value,
                    xeChannel.Element("icon").Attribute("src").Value
                    );

                channels.Add(channel);
            }
            return channels;
        }

        public Program getProgram(string programTitle)
        {
            throw new NotImplementedException();
        }

        public List<Program> getPrograms(string channelId)
        {
            List<XElement> programList = programData.Where(program => program.Attribute("id").Value == channelId).ToList();
            List<Program> programs = new List<Program>();
            string start = string.Empty, stop = string.Empty, title = string.Empty, desc = string.Empty, category = string.Empty, image = string.Empty;

            foreach (XElement xeProgram in programList)
            {
                start = xeProgram.Attribute("start") != null ? xeProgram.Attribute("start").Value : string.Empty;
                start = start.Substring(8, 2) + ":" + start.Substring(10, 2);
                stop = xeProgram.Attribute("stop") != null ? xeProgram.Attribute("stop").Value : string.Empty;
                stop = stop.Substring(8, 2) + ":" + stop.Substring(10, 2);

                title = xeProgram.Element("title") != null ? xeProgram.Element("title").Value : string.Empty;
                desc = xeProgram.Element("desc") != null ? xeProgram.Element("desc").Value : string.Empty;
                category = xeProgram.Element("category") != null ? xeProgram.Element("category").Value : string.Empty;
                image = xeProgram.Element("icon") != null ? xeProgram.Element("icon").Attribute("src").Value : string.Empty;

                Program program = new Program(start, stop, title, desc, category, image);

                programs.Add(program);
            }
            return programs;
        }
    }
}
