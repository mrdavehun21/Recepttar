namespace Recepttar.Server.BLL.DTOs.Poll
{
    public class PollDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<PollOptionDto> Options { get; set; }
        public int? VotedOn { get; set; }
    }
}
