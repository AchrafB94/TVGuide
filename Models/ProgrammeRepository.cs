using System.Xml.Linq;

namespace TVGuide.Models
{
    public class ProgrammeRepository : IProgrammeRepository
    {
        public static XDocument xdElCinema = XDocument.Load("https://iptv-org.github.io/epg/guides/eg-en/elcinema.com.epg.xml");
        public Programme GetCurrentProgram(int IdXMLChannel)
        {
            throw new NotImplementedException();
        }

        public List<Programme> GetProgrammesByChannel(string IdXMLChannel, string XML)
        {
            List<XElement> xeProgrammes = new List<XElement>();
           if (XML.Equals("ElCinema"))
            {
               xeProgrammes = xdElCinema.Root.Descendants("programme").Where(prg => prg.Attribute("channel").Value == IdXMLChannel).ToList();
            }

            List<Programme> list = new List<Programme>();
           foreach(XElement xeProgramme in xeProgrammes)
            {
                Programme programme = new Programme(
                    xeProgramme.Attribute("start").Value,
                    xeProgramme.Attribute("stop").Value,
                    xeProgramme.Element("title").Value,
                    xeProgramme.Element("desc").Value,
                    xeProgramme.Element("category").Value,
                    xeProgramme.Element("icon").Attribute("src").Value);
                list.Add(programme);
            }
            return list;
        }

        public List<Programme> GetProgrammesByNameAndDescription(string query)
        {
            throw new NotImplementedException();
        }
    }
}
