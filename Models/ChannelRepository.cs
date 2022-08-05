
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

    public List<Channel> getChannelsByCategory(string category)
    {
        return _context.Channels.Where(ch => ch.Category.Name == category).OrderBy(ch => ch.Position).ToList();
    }

    public List<Channel> getChannelsByPackage(string package)
    {
        return _context.Channels.Where(ch => ch.Package.Name == package).OrderBy(ch => ch.Position).ToList();
    }
    public List<Programme> GetCurrentProgrammes()
    {
        List<string?> ChannelXMLIds = _context.Channels.Select(ch => ch.IdXML).ToList();
        List<Programme> list = ProgrammeContext.list.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && prg.Start <= DateTime.Now && DateTime.Now < prg.Stop).ToList();

        foreach (Programme prg in list)
        {
            prg.Channel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
        }

        list = list.OrderBy(prg => prg.Channel.Position).DistinctBy(prg => prg.Channel.IdXML).ToList();
        return list;
    }

    public List<Programme> GetProgrammesByChannel(string IdXMLChannel)
    {
        return ProgrammeContext.list?.Where(prg => prg.ChannelName == IdXMLChannel).ToList();
    }

    public List<Programme> GetProgrammesByNameAndDescription(string query)
    {
        List<string?> ChannelXMLIds = _context.Channels.Select(ch => ch.IdXML).ToList();
        query = query.ToLower();
        List<Programme> list = ProgrammeContext.list.Where(prg => ChannelXMLIds.Contains(prg.ChannelName) && (prg.Title.ToLower().Contains(query) || prg.Description.ToLower().Contains(query)) && prg.Stop >= DateTime.Now).OrderBy(prg => prg.Start).ToList();


        foreach (Programme prg in list)
        {
            Channel programChannel = _context.Channels.Where(ch => ch.IdXML == prg.ChannelName).FirstOrDefault();
            prg.Channel = programChannel;
        }
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
        return GetProgrammesByChannel(channelXML).Where(prg => prg.Start >= DateTime.Today.AddHours(20)).Take(3).ToList();
    }
}