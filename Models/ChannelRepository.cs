
using System.Xml.Linq;
using TVGuide.Models;

public class ChannelRepository : IChannelRepository
{
    private readonly ChannelContext _context;
    public static XDocument xdElCinema = XDocument.Load("https://iptv-org.github.io/epg/guides/eg-en/elcinema.com.epg.xml");
    public static XDocument xdTVBlue = XDocument.Load("https://iptv-org.github.io/epg/guides/ch/tv.blue.ch.epg.xml");
    public static XDocument xdProgrammeTV = XDocument.Load("https://iptv-org.github.io/epg/guides/fr/programme-tv.net.epg.xml");
    public static XDocument xdOSN = XDocument.Load("https://iptv-org.github.io/epg/guides/dz-ar/osn.com.epg.xml");
    
    public ChannelRepository(ChannelContext appDbContext)
    {
        _context = appDbContext;
    }
    public List<Channel> getAllChannels()
    {
        return _context.Channels.OrderBy(ch => ch.Position).ToList();
    }

    public Channel getChannel(int channelId)
    {
        return _context.Channels.Where(ch => ch.Id == channelId).FirstOrDefault();
    }

    public List<Channel> getChannelsByCategory(string category)
    {
        return _context.Channels.Where(ch => ch.Category == category).ToList();
    }

    public List<Channel> getChannelsByPackage(string package)
    {
        return _context.Channels.Where(ch => ch.Package == package).ToList();
    }
    public List<Channel> getChannelsByPositions(int start, int end)
    {
        throw new NotImplementedException();
    }
    public Programme GetCurrentProgram(int IdXMLChannel)
    {
        throw new NotImplementedException();
    }

    public List<Programme> GetProgrammesByChannel(string IdXMLChannel, string XML)
    {
        List<XElement> xeProgrammes = new List<XElement>();

        switch (XML)
        {
            case "TVBlue": xeProgrammes = xdTVBlue.Root.Descendants("programme").Where(prg => prg.Attribute("channel").Value == IdXMLChannel).ToList(); break;
            case "ElCinema": xeProgrammes = xdElCinema.Root.Descendants("programme").Where(prg => prg.Attribute("channel").Value == IdXMLChannel).ToList(); break;
            case "ProgrammeTV": xeProgrammes = xdProgrammeTV.Root.Descendants("programme").Where(prg => prg.Attribute("channel").Value == IdXMLChannel).ToList(); break;
            case "OSN": xeProgrammes = xdOSN.Root.Descendants("programme").Where(prg => prg.Attribute("channel").Value == IdXMLChannel).ToList(); break;
        }

        List<Programme> list = new List<Programme>();

        foreach (XElement xeProgramme in xeProgrammes)
        {
            string start = xeProgramme.Attribute("start").Value;
            start = $"{start.Substring(0, 4)}-{start.Substring(4, 2)}-{start.Substring(6, 2)} {start.Substring(8, 2)}:{start.Substring(10, 2)}:00";
            DateTime startTime = Convert.ToDateTime(start);
            string stop = xeProgramme.Attribute("stop").Value;
            stop = $"{stop.Substring(0, 4)}-{stop.Substring(4, 2)}-{stop.Substring(6, 2)} {stop.Substring(8, 2)}:{stop.Substring(10, 2)}:00";
            DateTime stopTime = Convert.ToDateTime(stop);
            string title = xeProgramme.Element("title") != null ? xeProgramme.Element("title").Value : string.Empty;
            string desc = xeProgramme.Element("desc") != null ? xeProgramme.Element("desc").Value : string.Empty;
            string category = xeProgramme.Element("category") != null ? xeProgramme.Element("category").Value : string.Empty;
            string image = xeProgramme.Element("icon") != null ? xeProgramme.Element("icon").Attribute("src").Value : string.Empty;

            Programme programme = new Programme(startTime,stopTime,title,desc,category,image);
            list.Add(programme);
        }
        return list;
    }

    public List<Programme> GetProgrammesByNameAndDescription(string query)
    {
        List<Programme> list = new List<Programme>();
        List<XElement> xeList = xdElCinema.Root.Descendants("programme").Where(prg => prg.Attribute("title") != null && prg.Attribute("title").Value.Contains(query) || prg.Element("desc") != null && prg.Element("desc").Value.Contains(query)).ToList();
        foreach (XElement xeProgramme in xeList)
        {
            string IdXMLChannel = xeProgramme.Attribute("channel").Value;
            Channel programChannel = _context.Channels.Where(ch => ch.IdXML == IdXMLChannel).FirstOrDefault();

            if (programChannel != null)
            {
                string start = xeProgramme.Attribute("start").Value;
                start = $"{start.Substring(0, 4)}-{start.Substring(4, 2)}-{start.Substring(6, 2)} {start.Substring(8, 2)}:{start.Substring(10, 2)}";
                DateTime startTime = Convert.ToDateTime(start);
                string stop = xeProgramme.Attribute("stop").Value;
                stop = $"{stop.Substring(0, 4)}-{stop.Substring(4, 2)}-{stop.Substring(6, 2)} {stop.Substring(8, 2)}:{stop.Substring(10, 2)}";
                DateTime stopTime = Convert.ToDateTime(stop);
                string title = xeProgramme.Element("title") != null ? xeProgramme.Element("title").Value : string.Empty;
                string desc = xeProgramme.Element("desc") != null ? xeProgramme.Element("desc").Value : string.Empty;
                string category = xeProgramme.Element("category") != null ? xeProgramme.Element("category").Value : string.Empty;
                string image = xeProgramme.Element("icon") != null ? xeProgramme.Element("icon").Attribute("src").Value : string.Empty;

                Programme programme = new Programme(startTime, stopTime, title, desc, category, image, programChannel);
                list.Add(programme);
            }
        }
        return list;
    }
}