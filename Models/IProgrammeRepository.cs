namespace TVGuide.Models
{
    public interface IProgrammeRepository
    {
        List<Programme> GetProgrammesByChannel(string IdXMLChannel, string XML);

        Programme GetCurrentProgram(int IdXMLChannel);

        List<Programme> GetProgrammesByNameAndDescription(string query);
    }
}
