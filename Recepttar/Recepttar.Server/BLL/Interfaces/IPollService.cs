using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.DTOs.Poll;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IPollService
    {
        Task<List<PollCardDto>> GetActivePollsAsync(int userId);
        Task<Result> CreatePollAsync(int userId, PollDto pollDto);
        Task<Result> AddVoteAsync(int userId, int pollId, int optionId);
        Task<Result> DeletePollAsync(int userId, int pollId);
        Task<ResultT<UpdateResult>> UpdatePollAsync(int userId, int pollId, PollDto updateDto);
    }
}
