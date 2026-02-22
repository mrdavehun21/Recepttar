using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Constants;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Poll;
using Recepttar.Server.Enums;
using Recepttar.Server.Interfaces;
using Recepttar.Server.Models;

namespace Recepttar.Server.Services
{
    public class PollService : IPollService
    {
        private readonly AppDbContext _context;

        public PollService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PollDto>> GetActivePollsAsync(int userId)
        {
            var polls = await _context.Polls.ToListAsync();
            var options = await _context.PollOptions.ToListAsync();
            var votes = await _context.Votes.ToListAsync();

            var result = polls.Select(p => new PollDto
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                Question = p.Question,

                Options = options
                    .Where(o => o.PollId == p.Id)
                    .Select(o => new PollOptionDto
                    {
                        OptionId = o.Id,
                        OptionText = o.OptionText,
                        VoteCount = votes.Count(v => v.OptionId == o.Id)
                    }).ToList(),

                VotedOn = votes
                    .Where(v => v.UserId == userId && options.Any(o => o.PollId == p.Id && o.Id == v.OptionId))
                    .Select(v => (int?)v.OptionId)
                    .FirstOrDefault()
            }).ToList();

            return result;
        }

        public async Task<(bool success, string? error)> CreatePollAsync(int userId, PollDto pollDto)
        {
            var user = await _context.Users.FindAsync(userId);

            if(user == null)
            {
                return (false, Messages.Auth.UserNotFound);
            }

            if (user.Rank == UserRanksEnum.HomeCook || user.Rank == UserRanksEnum.ChefMaster)
            {
                return (false, Messages.Poll.LowRank);
            }

            if(string.IsNullOrWhiteSpace(pollDto.Question))
            {
                return (false, Messages.Poll.NoQuestion);
            }

            if (pollDto.Options.Count < 2)
            {
                return (false, Messages.Poll.LowOptions);
            }

            var poll = new Poll
            {
                AuthorId = userId,
                Question = pollDto.Question
            };

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            foreach (var optionDto in pollDto.Options)
            {
                _context.PollOptions.Add(new PollOption
                {
                    PollId = poll.Id,
                    OptionText = optionDto.OptionText
                });
            }
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool success, string? error)> AddVoteAsync(int userId, int pollId, int optionId)
        {
            var poll = await _context.Polls.FindAsync(pollId);

            if (poll == null)
            {
                return (false, Messages.Poll.NotFound);
            }
            
            var pollOptions = await _context.PollOptions
                .Where(po => po.PollId == pollId)
                .Select(po => po.Id)
                .ToListAsync();
            
            if (!pollOptions.Contains(optionId))
            {
                return (false, Messages.Poll.InvalidOption);
            }
            
            var existingVote = await _context.Votes.FirstOrDefaultAsync(v => v.UserId == userId && pollOptions.Contains(v.OptionId));
            
            if (existingVote != null)
            {
                return (false, Messages.Poll.Voted);
            }
            
            _context.Votes.Add(new Vote { UserId = userId, OptionId = optionId });
            await _context.SaveChangesAsync();
            
            return (true, null);
        }

        public async Task<(bool success, string? error)> DeletePollAsync(int userId, int pollId)
        {
            var poll = await _context.Polls.FindAsync(pollId);

            if (poll == null)
            {
                return (false, Messages.Poll.NotFound);
            }

            if (poll.AuthorId != userId)
            {
                return (false, Messages.Poll.NotOwnerDelete);
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdatePollAsync(int userId, int pollId, PollDto updateDto)
        {
            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll == null)
            {
                return (false, false, Messages.Poll.NotFound);
            }

            if (poll.AuthorId != userId)
            {
                return (false, false, Messages.Poll.NotOwner);
            }

            bool wasUpdated = false;

            // Update question
            if (!string.IsNullOrWhiteSpace(updateDto.Question) && updateDto.Question != poll.Question)
            {
                poll.Question = updateDto.Question;
                wasUpdated = true;
            }

            // Update options
            if (updateDto.Options != null && updateDto.Options.Count > 0)
            {
                var existingOptions = _context.PollOptions.Where(o => o.PollId == pollId);

                _context.PollOptions.RemoveRange(existingOptions);

                foreach (var optionDto in updateDto.Options)
                {
                    _context.PollOptions.Add(new PollOption
                    {
                        PollId = pollId,
                        OptionText = optionDto.OptionText
                    });
                }

                wasUpdated = true;
            }

            if (wasUpdated)
            {
                await _context.SaveChangesAsync();
            }

            return (true, wasUpdated, null);
        }

    }
}
