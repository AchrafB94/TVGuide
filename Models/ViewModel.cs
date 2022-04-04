namespace TVGuide.Models
{
    public class ViewModel
    {
        public Channel channel { get; set; }
        public IEnumerable<Programme> programs { get; set; }
        public ViewModel()
        {

        }
    }
}
