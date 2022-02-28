namespace TVGuide.Models
{
    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }

        public Channel(string id, string name, string logo)
        {
            this.id = id;
            this.name = name;
            this.logo = logo;
        }

    }
}
