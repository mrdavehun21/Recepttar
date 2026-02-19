using Recepttar.Server.DTOs.Poll;

namespace Recepttar.Server.Interfaces
{
    public interface IPollService
    {
        public Task<List<PollDto>> GetActivePollsAsync(int userId);

        public Task<(bool success, string? error)> CreatePollAsync(int userId, PollDto pollDto);

        public Task<(bool success, string? error)> AddVoteAsync(int userId, int pollId, int optionId);

        public Task<(bool success, string? error)> DeletePollAsync(int userId, int pollId);

        public Task<(bool success, bool wasUpdated, string? error)> UpdatePollAsync(int userId, int pollId, PollDto updateDto);
    }
}
