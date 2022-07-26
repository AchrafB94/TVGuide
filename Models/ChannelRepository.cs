
using System.Xml.Linq;
using TVGuide.Models;

public class ChannelRepository : IChannelRepository
{
    private readonly ChannelContext _context;
    public static XDocument? xdData;
    public ChannelRepository(ChannelContext appDbContext)
    {
        _context = appDbContext;
    }

    public static async Task SetupXMLData()
    {
        xdData = new XDocument(new XElement("tv"));
        XDocument xdSource;

        List<string> xmlSources = new List<string>();
        xmlSources.Add("https://iptv-org.github.io/epg/guides/eg-ar/elcinema.com.epg.xml");
        xmlSources.Add("https://iptv-org.github.io/epg/guides/qa/bein.com.epg.xml");
        xmlSources.Add("https://iptv-org.github.io/epg/guides/dz-ar/osn.com.epg.xml");
        xmlSources.Add("https://iptv-org.github.io/epg/guides/fr/telecablesat.fr.epg.xml");

        foreach (string source in xmlSources)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(source, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(stream))
                {
                    xdSource = await XDocument.LoadAsync(streamReader, LoadOptions.PreserveWhitespace,CancellationToken.None);
                    xdData.Root?.Add(xdSource.Root?.Elements("programme"));
                }
            }
        }
    }
    public List<Channel> getAllChannels()
    {
        return _context.Channels.OrderBy(ch => ch.Position).ToList();
    }

    public Channel getChannel(int channelId)
    {
        return _context.Channels.Where(ch => ch.Id == channelId).First();
    }

    public List<Channel> getChannelsByCategory(string category)
    {
        return _context.Channels.Where(ch => ch.Category == category).ToList();
    }

    public List<Channel> getChannelsByPackage(string package)
    {
        return _context.Channels.Where(ch => ch.Package == package).ToList();
    }
    public List<Programme> GetCurrentProgrammes()
    {
        List<Programme> list = new List<Programme>();
        List<string?> ChannelXMLIds = _context.Channels.Select(ch => ch.IdXML).ToList();


        List<XElement> xeList = new List<XElement>();
        xeList.AddRange(xdData.Root.Descendants("programme").Where(prg => ChannelXMLIds.Contains(prg.Attribute("channel").Value)).ToList());


        foreach (XElement xeProgramme in xeList)
        {
            string start = xeProgramme.Attribute("start").Value;
            start = $"{start.Substring(0, 4)}-{start.Substring(4, 2)}-{start.Substring(6, 2)} {start.Substring(8, 2)}:{start.Substring(10, 2)}:00";
            DateTime startTime = Convert.ToDateTime(start);
            string stop = xeProgramme.Attribute("stop").Value;
            stop = $"{stop.Substring(0, 4)}-{stop.Substring(4, 2)}-{stop.Substring(6, 2)} {stop.Substring(8, 2)}:{stop.Substring(10, 2)}:00";
            DateTime stopTime = Convert.ToDateTime(stop);

            if(startTime <= DateTime.Now && DateTime.Now < stopTime)
            {
                string title = xeProgramme.Element("title") != null ? xeProgramme.Element("title").Value : string.Empty;
                string desc = xeProgramme.Element("desc") != null ? xeProgramme.Element("desc").Value : string.Empty;
                string category = xeProgramme.Element("category") != null ? xeProgramme.Element("category").Value : string.Empty;
                string image = xeProgramme.Element("icon") != null ? xeProgramme.Element("icon").Attribute("src").Value : string.Empty;

                string IdXMLChannel = xeProgramme.Attribute("channel").Value;
                Channel programChannel = _context.Channels.Where(ch => ch.IdXML == IdXMLChannel).FirstOrDefault();

                Programme programme = new Programme(startTime, stopTime, title, desc, category, image, programChannel);
                list.Add(programme);
            }
        }
        list = list.OrderBy(prg => prg.Channel.Position).ToList();
        list = list.DistinctBy(prg => prg.Channel.IdXML).ToList();
        return list;
    }

    public List<Programme> GetProgrammesByChannel(string IdXMLChannel)
    {

        List<XElement> xeProgrammes = xdData.Root.Descendants("programme").Where(prg => prg.Attribute("channel").Value == IdXMLChannel).ToList();

        List<Programme> list =
        (from programme in xeProgrammes
         select new Programme { 
             Start = Convert.ToDateTime($"{programme.Attribute("start").Value.Substring(0, 4)}-{programme.Attribute("start").Value.Substring(4, 2)}-{programme.Attribute("start").Value.Substring(6, 2)} {programme.Attribute("start").Value.Substring(8, 2)}:{programme.Attribute("start").Value.Substring(10, 2)}:00"),
             Stop = Convert.ToDateTime($"{programme.Attribute("stop").Value.Substring(0, 4)}-{programme.Attribute("stop").Value.Substring(4, 2)}-{programme.Attribute("stop").Value.Substring(6, 2)} {programme.Attribute("stop").Value.Substring(8, 2)}:{programme.Attribute("stop").Value.Substring(10, 2)}:00"),
             Title = programme.Element("title").Value,
             Description = programme.Element("desc") != null ? programme.Element("desc").Value : string.Empty,
             Category = programme.Element("category") != null ? programme.Element("category").Value : string.Empty,
             Image = programme.Element("icon") != null ? programme.Element("icon").Attribute("src").Value : string.Empty
         }).Where(prg => prg.Stop < DateTime.Today.AddDays(1)).ToList();

        return list;
    }

    public List<Programme> GetProgrammesByNameAndDescription(string query)
    {

        List<Programme> list = new List<Programme>();
        List<XElement> xeList = xdData.Root.Descendants("programme").Where(prg => (prg.Element("title") != null && prg.Element("title").Value.Contains(query)) || (prg.Element("desc") != null && prg.Element("desc").Value.Contains(query))).OrderBy(prg => prg.Attribute("start").Value).ToList();

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
        list = list.OrderBy(prg => prg.Start).ToList();
        return list;
    }

    public Channel getRandomChannel()
    {
        var rand = new Random();
        int toSkip = rand.Next(0, _context.Channels.Count());
        return _context.Channels.Skip(toSkip).Take(1).First();
    }

    public List<Programme> GetTonightProgrammes(string channelXML)
    {
        DateTime.Today.AddHours(20);
        return GetProgrammesByChannel(channelXML).Where(prg => prg.Start >= DateTime.Today.AddHours(20)).Take(4).ToList();
    }
}