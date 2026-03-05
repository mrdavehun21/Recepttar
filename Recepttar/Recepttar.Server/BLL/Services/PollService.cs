using AutoMapper;
using Recepttar.Server.BLL.Common;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Poll;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.BLL.Interfaces;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Services
{
    public class PollService : IPollService
    {
        private readonly IPollRepository _pollRepository;
        private readonly IMapper _mapper;

        private const int MinPollOptions = 2;

        public PollService(IPollRepository pollRepository, IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<List<PollCardDto>> GetActivePollsAsync(int userId)
        {
            var polls = await _pollRepository.GetAllAsync();
            return polls.Select(p => new PollCardDto
            {
                Id = p.Id,
                AuthorId = p.Author.Id,
                FullName = p.Author.FullName,
                ProfilePicture = ProfilePicturePath.GetPath(p.Author.Id),
                Question = p.Question,
                Options = p.Options.Select(o => new PollOptionDto
                {
                    OptionId = o.Id,
                    OptionText = o.OptionText,
                    VoteCount = o.Votes.Count
                }).ToList(),
                VotedOn = p.Options
                    .SelectMany(o => o.Votes.Where(v => v.UserId == userId).Select(v => (int?)o.Id))
                    .FirstOrDefault()
            }).ToList();
        }

        public async Task<Result> CreatePollAsync(int userId, PollDto pollDto)
        {
            var user = await _pollRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure(Messages.Auth.UserNotFound);
            }

            if (user.Rank == UserRanksEnum.HomeCook || user.Rank == UserRanksEnum.ChefMaster)
            {
                return Result.Failure(Messages.Poll.LowRank);
            }

            if (string.IsNullOrWhiteSpace(pollDto.Question))
            {
                return Result.Failure(Messages.Poll.NoQuestion);
            }

            if (pollDto.Options.Count < MinPollOptions)
            {
                return Result.Failure(Messages.Poll.LowOptions);
            }

            var poll = new Poll
            {
                AuthorId = userId,
                Question = pollDto.Question,
                Options = pollDto.Options.Select(o => _mapper.Map<PollOption>(o)).ToList()
            };

            await _pollRepository.AddPollAsync(poll);
            return Result.Success(Messages.Poll.Created);
        }

        public async Task<Result> AddVoteAsync(int userId, int pollId, int optionId)
        {
            var poll = await _pollRepository.GetByIdAsync(pollId);
            if (poll == null)
            {
                return Result.Failure(Messages.Poll.NotFound);
            }

            if (!await _pollRepository.OptionBelongsToPollAsync(pollId, optionId))
            {
                return Result.Failure(Messages.Poll.InvalidOption);
            }

            if (await _pollRepository.GetExistingVoteAsync(userId, pollId) != null)
            {
                return Result.Failure(Messages.Poll.Voted);
            }

            await _pollRepository.AddVoteAsync(new Vote { UserId = userId, OptionId = optionId });
            return Result.Success(Messages.Poll.Recorded);
        }

        public async Task<Result> DeletePollAsync(int userId, int pollId)
        {
            var poll = await _pollRepository.GetByIdAsync(pollId);
            if (poll == null)
            {
                return Result.Failure(Messages.Poll.NotFound);
            }

            if (poll.AuthorId != userId)
            {
                return Result.Failure(Messages.Poll.NotOwnerDelete);
            }

            await _pollRepository.DeletePollAsync(poll);
            return Result.Success(null);
        }

        public async Task<ResultT<UpdateResult>> UpdatePollAsync(int userId, int pollId, PollDto updateDto)
        {
            var poll = await _pollRepository.GetByIdAsync(pollId);
            if (poll == null)
            {
                return ResultT<UpdateResult>.Failure(Messages.Poll.NotFound);
            }

            if (poll.AuthorId != userId)
            {
                return ResultT<UpdateResult>.Failure(Messages.Poll.NotOwner);
            }

            bool wasUpdated = false;

            if (!string.IsNullOrWhiteSpace(updateDto.Question) && updateDto.Question != poll.Question)
            {
                poll.Question = updateDto.Question;
                wasUpdated = true;
            }

            if (updateDto.Options != null && updateDto.Options.Count > 0)
            {
                var options = updateDto.Options.Select(o =>
                {
                    var option = _mapper.Map<PollOption>(o);
                    option.PollId = pollId;
                    return option;
                }).ToList();

                await _pollRepository.ReplaceOptionsAsync(pollId, options);
                wasUpdated = true;
            }

            if (wasUpdated)
            {
                await _pollRepository.UpdatePollAsync(poll);
                return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = true }, Messages.Poll.Updated);
            }

            return ResultT<UpdateResult>.Success(new UpdateResult { WasUpdated = false }, Messages.Poll.NoChanges);
        }
    }
}
