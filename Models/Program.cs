namespace TVGuide.Models
{
    public class Program
    {
        public string Start { get; set; }
        public string Stop { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public Program(string Start, string Stop, string Title, string Description, string Category, string Image)
        {
            this.Start = Start;
            this.Stop = Stop;
            this.Title = Title;
            this.Description = Description;
            this.Category = Category;
            this.Image = Image;
        }

    }
}
