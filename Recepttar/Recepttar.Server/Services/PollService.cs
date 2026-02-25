using Recepttar.Server.DTOs.Poll;
using Recepttar.Server.Interfaces.Repositories;
using Recepttar.Server.Interfaces.Services;

namespace Recepttar.Server.Services
{
    public class PollService : IPollService
    {
        private readonly IPollRepository _pollRepository;

        public PollService(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public async Task<List<PollCardDto>> GetActivePollsAsync(int userId)
        {
            return await _pollRepository.GetActivePollsAsync(userId);
        }

        public async Task<(bool success, string? error)> CreatePollAsync(int userId, PollDto pollDto)
        {
            return await _pollRepository.CreatePollAsync(userId, pollDto);
        }

        public async Task<(bool success, string? error)> AddVoteAsync(int userId, int pollId, int optionId)
        {
            return await _pollRepository.AddVoteAsync(userId, pollId, optionId);
        }

        public async Task<(bool success, string? error)> DeletePollAsync(int userId, int pollId)
        {
            return await _pollRepository.DeletePollAsync(userId, pollId);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdatePollAsync(int userId, int pollId, PollDto updateDto)
        {
            return await _pollRepository.UpdatePollAsync(userId, pollId, updateDto);
        }
    }
}
