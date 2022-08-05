using System.Xml.Linq;

namespace TVGuide.Models
{
    public class ProgrammeContext
    {
        public static List<Programme>? list;

        public static async Task Setup()
        {
            XDocument? xdData = new XDocument(new XElement("tv"));
            List<string> xmlSources = new List<string>();
            XDocument xdSource;


            xmlSources.Add("https://iptv-org.github.io/epg/guides/eg-en/elcinema.com.epg.xml");
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
                        xdSource = await XDocument.LoadAsync(streamReader, LoadOptions.PreserveWhitespace, CancellationToken.None);
                        xdData.Root?.Add(xdSource.Root?.Elements("programme"));
                    }
                }
            }

            list = (from programme in xdData.Root?.Elements("programme")
                    select new Programme
             {
                 Start = Convert.ToDateTime($"{programme.Attribute("start").Value.Substring(0, 4)}-{programme.Attribute("start").Value.Substring(4, 2)}-{programme.Attribute("start").Value.Substring(6, 2)} {programme.Attribute("start").Value.Substring(8, 2)}:{programme.Attribute("start").Value.Substring(10, 2)}:00"),
                 Stop = Convert.ToDateTime($"{programme.Attribute("stop").Value.Substring(0, 4)}-{programme.Attribute("stop").Value.Substring(4, 2)}-{programme.Attribute("stop").Value.Substring(6, 2)} {programme.Attribute("stop").Value.Substring(8, 2)}:{programme.Attribute("stop").Value.Substring(10, 2)}:00"),
                 Title = programme.Element("title").Value,
                 Description = programme.Element("desc") != null ? programme.Element("desc").Value : string.Empty,
                 Category = programme.Element("category") != null ? programme.Element("category").Value : string.Empty,
                 Image = programme.Element("icon") != null ? programme.Element("icon").Attribute("src").Value : string.Empty,
                 ChannelName = programme.Attribute("channel").Value
             }).ToList();
        }

    }
}
