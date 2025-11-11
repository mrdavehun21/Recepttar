namespace Recepttar.Server.DTO.PollDTO
{
    public class ActivePoll
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<PollOption> Options { get; set; }
    }
}
