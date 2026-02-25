using Recepttar.Server.DTOs.Poll;

namespace Recepttar.Server.Interfaces.Repositories
{
    public interface IPollRepository
    {
        Task<List<PollCardDto>> GetActivePollsAsync(int userId);
        Task<(bool success, string? error)> CreatePollAsync(int userId, PollDto pollDto);
        Task<(bool success, string? error)> AddVoteAsync(int userId, int pollId, int optionId);
        Task<(bool success, string? error)> DeletePollAsync(int userId, int pollId);
        Task<(bool success, bool wasUpdated, string? error)> UpdatePollAsync(int userId, int pollId, PollDto updateDto);
    }
}
