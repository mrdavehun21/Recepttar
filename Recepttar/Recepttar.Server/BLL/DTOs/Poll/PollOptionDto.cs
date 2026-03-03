namespace Recepttar.Server.BLL.DTOs.Poll
{
    public class PollOptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public int VoteCount { get; set; }
    }
}
