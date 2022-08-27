using System.IO.Compression;
using System.Net;
using System.Xml.Linq;

namespace TVGuide.Models
{
    public class ProgrammeContext
    {
        public static List<Programme> list;

        public static async Task Setup(WebApplicationBuilder builder)
        {
            XDocument? xdData = new XDocument(new XElement("tv"));
            List<string> xmlSources = new List<string>();
            XDocument xdSource;

            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:Elcinema"));
            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:Bein"));
            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:OSN"));
            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:Canal"));
            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:HDPlus"));
            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:Mediaset"));
            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:Rai"));
            xmlSources.Add(builder.Configuration.GetValue<string>("Sources:Movistar"));

            foreach (string source in xmlSources)
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(source, HttpCompletionOption.ResponseHeadersRead);

                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var streamReader = new StreamReader(stream))
                    {
                        GZipStream zip = new GZipStream(stream, CompressionMode.Decompress);
                        xdSource = XDocument.Load(zip);
                        xdData.Root?.Add(xdSource.Root?.Elements("programme"));
                    }
                }
            }

            list = (from programme in xdData.Root?.Elements("programme")
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

    }
}
