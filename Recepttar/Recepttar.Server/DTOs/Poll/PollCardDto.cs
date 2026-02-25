namespace Recepttar.Server.DTOs.Poll
{
    public class PollCardDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public List<PollOptionDto> Options { get; set; }
        public int? VotedOn { get; set; }
    }
}