namespace TVGuide.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Channel> Channels { get; set; } 

    }
}