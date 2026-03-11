using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IPollRepository
    {
        Task<IEnumerable<Poll>> GetAllAsync();
        Task<IEnumerable<Poll>> GetPollsByUserId(int userId);
        Task<Poll?> GetByIdAsync(int pollId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> OptionBelongsToPollAsync(int pollId, int optionId);
        Task<Vote?> GetExistingVoteAsync(int userId, int pollId);
        Task AddPollAsync(Poll poll);
        Task AddVoteAsync(Vote vote);
        Task DeletePollAsync(Poll poll);
        Task ReplaceOptionsAsync(int pollId, IEnumerable<PollOption> options);
        Task UpdatePollAsync(Poll poll);
    }
}
