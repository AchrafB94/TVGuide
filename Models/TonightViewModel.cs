namespace TVGuide.Models
{
    public class TonightViewModel
    {
        public Channel channel { get; set; }
        public List<Programme> programs { get; set; }
        public TonightViewModel()
        {

        }
    }
}
