namespace TVGuide.Models
{
    public class Programme
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public Channel? Channel { get; set; }

        public Programme()
        {

        }
        public Programme(DateTime Start, DateTime Stop, string Title, string Description, string Category, string Image)
        {
            this.Start = Start;
            this.Stop = Stop;
            this.Title = Title;
            this.Description = Description;
            this.Category = Category;
            this.Image = Image;
        }
        public Programme(DateTime Start, DateTime Stop, string Title, string Description, string Category, string Image, Channel channel)
        {
            this.Start = Start;
            this.Stop = Stop;
            this.Title = Title;
            this.Description = Description;
            this.Category = Category;
            this.Image = Image;
            this.Channel = channel;
        }

    }
}
