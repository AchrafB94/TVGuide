namespace TVGuide.Models
{
    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public string logo { get; set; }

        public Channel(string id, string name, string website, string logo)
        {
            this.id = id;
            this.name = name;
            this.website = website;
            this.logo = logo;
        }

    }
}
