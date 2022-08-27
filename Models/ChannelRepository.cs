
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TVGuide.Models;

public class ChannelRepository : IChannelRepository
{
    private readonly ChannelContext _context;
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
        return _context.Channels.Where(ch => ch.Id == channelId).First();
    }

    public List<Channel> getChannelsByCategory(int category)
    {
        return _context.Channels.Where(ch => ch.Category.Id == category).OrderBy(ch => ch.Name).ToList();
    }

    public List<Channel> getChannelsByPackage(int package)
    {
        return _context.Channels.Where(ch => ch.Package.Id == package).OrderBy(ch => ch.Position).ToList();
    }
    public async Task<List<Programme>> GetCurrentProgrammes()
    {
        List<string?> ChannelXMLIds = await _context.Channels.Where(ch => (ch.Package == null || ch.Package.Name == "OSN" || ch.Package.Name == "beIN")).Select(ch => ch.IdXML).ToListAsync();
        List<Programme> list = ProgrammeContext.list.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && prg.Start <= DateTime.Now && DateTime.Now < prg.Stop ).ToList();

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
        List<Programme> list = ProgrammeContext.list.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && prg.Start <= DateTime.Now && DateTime.Now < prg.Stop).ToList();

        foreach (Programme prg in list)
        {
            prg.Channel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
        }

        list = list.OrderBy(prg => prg.Channel.Position).DistinctBy(prg => prg.Channel.IdXML).ToList();
        return list;
    }

    public List<IGrouping<DayOfWeek,Programme>> GetProgrammesByChannel(string IdXMLChannel)
    {
        return ProgrammeContext.list?.Where(prg => prg.ChannelName == IdXMLChannel).GroupBy(prg => prg.Start.DayOfWeek).ToList();
    }

    public List<Programme> GetProgrammesByNameAndDescription(string query)
    {
        List<string?> ChannelXMLIds = _context.Channels.Select(ch => ch.IdXML).ToList();
        query = query.ToLower();
        List<Programme> list = ProgrammeContext.list.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && ((!string.IsNullOrEmpty(prg.Title) && prg.Title.ToLower().Contains(query)) || (!string.IsNullOrEmpty(prg.Description) && prg.Description.ToLower().Contains(query)) || (!string.IsNullOrEmpty(prg.Category) && prg.Category.ToLower().Contains(query))) && prg.Stop >= DateTime.Now).OrderBy(prg => prg.Start).ToList();


        foreach (Programme prg in list)
        {
            Channel programChannel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
            prg.Channel = programChannel;
        }
        return list;
    }


    public TonightViewModel GetTonightProgrammes()
    {
        var rand = new Random();
        int toSkip = rand.Next(0, _context.Channels.Where(ch => ch.Package.Name == "Canal" || ch.Name.Contains("Rai")).Count());
        Channel randomChannel = _context.Channels.Where(ch => ch.Package.Name == "Canal" || ch.Name.Contains("Rai")).Skip(toSkip).Take(1).First();
        var programmes = ProgrammeContext.list.Where(prg => prg.ChannelName == randomChannel.IdXML && prg.Start >= DateTime.Today.AddHours(19)).Take(5).ToList();
        TonightViewModel model = new TonightViewModel();
        model.programs = programmes;
        model.channel = randomChannel;
        return model;
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
            userProgrammes.AddRange(ProgrammeContext.list.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && !string.IsNullOrEmpty(prg.Title) && prg.Title.ToLower().Contains(keyword) && prg.Stop >= DateTime.Now).ToList());
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
}