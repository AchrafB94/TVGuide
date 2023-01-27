
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO.Compression;
using System.Xml.Linq;
using TVGuide.Models;

public class ChannelRepository : IChannelRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private List<Programme> _programmes;
    public ChannelRepository(ApplicationDbContext appDbContext, IConfiguration configuration)
    {
        _context = appDbContext;
        _configuration = configuration;
        FileInfo file = new FileInfo("xml/programmes.xml");
        DateTime fileDate = file.LastWriteTime;
        XDocument xdData;

        if (DateTime.Today > fileDate)
            xdData = SetupProgrammes().GetAwaiter().GetResult();
        else
            xdData = XDocument.Load("xml/programmes.xml");

        _programmes = (from programme in xdData.Root?.Elements("programme")
                       select new Programme
                       {
                           Start = Convert.ToDateTime($"{programme.Attribute("start").Value.Substring(0, 4)}-{programme.Attribute("start").Value.Substring(4, 2)}-{programme.Attribute("start").Value.Substring(6, 2)} {programme.Attribute("start").Value.Substring(8, 2)}:{programme.Attribute("start").Value.Substring(10, 2)}:00"),
                           Stop = Convert.ToDateTime($"{programme.Attribute("stop").Value.Substring(0, 4)}-{programme.Attribute("stop").Value.Substring(4, 2)}-{programme.Attribute("stop").Value.Substring(6, 2)} {programme.Attribute("stop").Value.Substring(8, 2)}:{programme.Attribute("stop").Value.Substring(10, 2)}:00"),
                           Title = programme.Element("title")?.Value,
                           Description = programme.Element("desc")?.Value,
                           Category = programme.Element("category")?.Value,
                           Image = programme.Element("icon")?.Attribute("src")?.Value,
                           ChannelName = programme.Attribute("channel")?.Value
                       }).ToList();
    }

    public async Task<XDocument> SetupProgrammes()
    {
        XDocument xdData = new XDocument(new XElement("tv"));
        List<string> xmlSources = new List<string>();
        XDocument xdSource;

        xmlSources.Add(_configuration.GetValue<string>("Sources:Elcinema"));
        xmlSources.Add(_configuration.GetValue<string>("Sources:Bein"));
        xmlSources.Add(_configuration.GetValue<string>("Sources:Canal"));
        xmlSources.Add(_configuration.GetValue<string>("Sources:HDPlus"));
        xmlSources.Add(_configuration.GetValue<string>("Sources:Tivu"));
        xmlSources.Add(_configuration.GetValue<string>("Sources:Sky"));
        xmlSources.Add(_configuration.GetValue<string>("Sources:Movistar"));

        foreach (string source in xmlSources)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(source, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(stream))
                {
                    //GZipStream zip = new GZipStream(stream, CompressionMode.Decompress);
                    xdSource = XDocument.Load(streamReader);
                    xdData.Root.Add(xdSource.Root?.Elements("programme"));
                }
            }
        }
        var channelXMLids = _context.Channels.Select(ch => ch.IdXML).ToList();
        xdData.Root.Elements("programme").Where(prg => !channelXMLids.Contains(prg.Attribute("channel").Value)).Remove();
        xdData.Save("xml/programmes.xml");
        return xdData;
    }
    public List<Channel> getAllChannels()
    {
        return _context.Channels.OrderBy(ch => ch.Position).ToList();
    }

    public Channel getChannel(int channelId)
    {
        return _context.Channels.Include(ch => ch.Package).Where(ch => ch.Id == channelId).First();
    }

    public List<Channel> getChannelsByCategory(int category)
    {
        return _context.Channels.Where(ch => ch.Category.Id == category).OrderBy(ch => ch.Name).ToList();
    }

    public List<Channel> getChannelsByPackage(int package)
    {
        return _context.Channels.Include(ch => ch.Package).Where(ch => ch.Package.Id == package).OrderBy(ch => ch.Position).ToList();
    }
    public async Task<List<Programme>> GetCurrentProgrammes()
    {
        var culture = CultureInfo.CurrentCulture.Name;
        List<string?> ChannelXMLIds = new List<string?>();
        if (culture == "ar")
            ChannelXMLIds = await _context.Channels.Where(ch => (ch.Package == null || ch.Package.Name == "OSN" || ch.Package.Name == "beIN")).Select(ch => ch.IdXML).ToListAsync();
        else if (culture == "fr")
            ChannelXMLIds = await _context.Channels.Where(ch => (ch.Package.Name == "Canal" || ch.Package.Name == "Hotbird")).Select(ch => ch.IdXML).ToListAsync();


        List<Programme> list = _programmes.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && prg.Start <= DateTime.Now && DateTime.Now < prg.Stop ).ToList();

        foreach (Programme prg in list)
        {
            prg.Channel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
        }

        list = list.OrderBy(prg => prg.Channel.Position).DistinctBy(prg => prg.Channel.IdXML).ToList();
        return list;
    }

    public async Task<List<Programme>> GetCurrentProgrammes(int IdCategory)
    {
        List<string?> ChannelXMLIds = await _context.Channels.Where(ch => ch.Category.Id == IdCategory).Select(ch => ch.IdXML).ToListAsync();
        List<Programme> list = _programmes.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && prg.Start <= DateTime.Now && DateTime.Now < prg.Stop).ToList();

        foreach (Programme prg in list)
        {
            prg.Channel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
        }

        list = list.OrderBy(prg => prg.Channel.Position).DistinctBy(prg => prg.Channel.IdXML).ToList();
        return list;
    }

    public List<IGrouping<DayOfWeek,Programme>> GetProgrammesByChannel(string IdXMLChannel)
    {
        return _programmes.Where(prg => prg.ChannelName == IdXMLChannel).GroupBy(prg => prg.Start.DayOfWeek).ToList();
    }

    public List<Programme> GetProgrammesByNameAndDescription(string query)
    {
        List<string?> ChannelXMLIds = _context.Channels.Select(ch => ch.IdXML).ToList();
        query = query.ToLower();
        List<Programme> list = _programmes.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && ((!string.IsNullOrEmpty(prg.Title) && prg.Title.ToLower().Contains(query)) || (!string.IsNullOrEmpty(prg.Description) && prg.Description.ToLower().Contains(query)) || (!string.IsNullOrEmpty(prg.Category) && prg.Category.ToLower().Contains(query))) && prg.Stop >= DateTime.Now).OrderBy(prg => prg.Start).ToList();


        foreach (Programme prg in list)
        {
            Channel programChannel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
            prg.Channel = programChannel;
        }
        return list;
    }


    public TonightViewModel GetTonightProgrammes()
    {
        var channels = _context.Channels.Where(ch => (ch.Package.Name == "Canal" || ch.Name.Contains("Rai")) && !ch.Name.Contains("MAX") && !ch.Name.Contains("360") && !ch.Name.Contains("MultiSports"));
        var rand = new Random();
        int toSkip = rand.Next(0, channels.Count());
        Channel randomChannel = channels.Skip(toSkip).Take(1).First();
        var programmes = _programmes.Where(prg => prg.ChannelName == randomChannel.IdXML && prg.Start >= DateTime.Today.AddHours(20)).FirstOrDefault();
        if (programmes != null)
        {
            programmes.Channel = randomChannel;
            TonightViewModel model = new TonightViewModel();
            model.programme = programmes;
            model.channel = randomChannel;
            return model;
        }
        else
            return new TonightViewModel();

    }

    public string GetCategoryName(int IdCategory)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == IdCategory).Name;
    }

    public string GetPackageName(int IdPackage)
    {
        return _context.Packages.FirstOrDefault(p => p.Id == IdPackage).Name;
    }

    public List<Programme> GetUserProgrammes(string keywords)
    {
        List<string?> ChannelXMLIds = _context.Channels.Select(ch => ch.IdXML).ToList();
        List<Programme> userProgrammes = new List<Programme>();
        
        foreach(string keyword in keywords.Split(';'))
        {
            userProgrammes.AddRange(_programmes.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && !string.IsNullOrEmpty(prg.Title) && prg.Title.ToLower().Contains(keyword) && prg.Stop >= DateTime.Now).ToList());
        }

        foreach(var prg in userProgrammes)
        {
            prg.Channel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
        }

        userProgrammes = userProgrammes.OrderBy(prg => prg.Start).ToList();
        return userProgrammes;
        
    }
    public List<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    public List<FavoriteChannel> GetUserFavoriteChannels(TVGuideUser user)
    {
       return _context.FavoriteChannels.Include(fc => fc.Channel).Where(fc => fc.User == user).ToList();
    }

    public List<Programme> GetCurrentProgrammesByChannel(string idXMLChannel)
    {
        return _programmes.Where(prg => prg.ChannelName == idXMLChannel && prg.Start >= DateTime.Now).Take(2).ToList();
    }

    public void AddFavoriteChannel(TVGuideUser user, int IdChannel)
    {
        var channel = _context.Channels.Find(IdChannel);
        int position = _context.FavoriteChannels.Where(fc => fc.User == user).Count() + 1;
        FavoriteChannel fc = new FavoriteChannel(user, channel, position);
        _context.FavoriteChannels.Add(fc);
        _context.SaveChanges();
    }
}