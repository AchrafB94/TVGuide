namespace TVGuide.Models
{
    public class ViewModel
    {
        public Channel channel { get; set; }
        public IEnumerable<Program> programs { get; set; }
        public ViewModel()
        {

        }
    }
}
