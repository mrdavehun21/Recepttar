using Recepttar.Server.BLL.DTOs.Poll;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IPollService
    {
        Task<List<PollCardDto>> GetActivePollsAsync(int userId);
        Task<(bool success, string? error)> CreatePollAsync(int userId, PollDto pollDto);
        Task<(bool success, string? error)> AddVoteAsync(int userId, int pollId, int optionId);
        Task<(bool success, string? error)> DeletePollAsync(int userId, int pollId);
        Task<(bool success, bool wasUpdated, string? error)> UpdatePollAsync(int userId, int pollId, PollDto updateDto);
    }
}
