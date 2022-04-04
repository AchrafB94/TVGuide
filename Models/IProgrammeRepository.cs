namespace TVGuide.Models
{
    public interface IProgrammeRepository
    {
        List<Programme> GetProgrammesByChannel(int IdXMLChannel);

        Programme GetCurrentProgram(int IdXMLChannel);

        List<Programme> GetProgrammesByNameAndDescription(string query);
    }
}
