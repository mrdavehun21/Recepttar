using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Poll;
using Recepttar.Server.Enums;
using Recepttar.Server.Models;

namespace Recepttar.Server.Services
{
    public class PollService
    {
        private readonly AppDbContext _context;

        public PollService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PollDto>> GetActivePollsAsync(int userId)
        {
            var polls = await _context.Poll.ToListAsync();
            var options = await _context.PollOption.ToListAsync();
            var votes = await _context.Vote.ToListAsync();

            var result = polls.Select(p => new PollDto
            {
                Id = p.Id,
                Question = p.Question,

                Options = options
                    .Where(o => o.PollId == p.Id)
                    .Select(o => new PollOptionDto
                    {
                        OptionId = o.Id,
                        OptionText = o.OptionText,
                        VoteCount = votes.Count(v => v.OptionId == o.Id && v.PollId == p.Id)
                    }).ToList(),

                VotedOn = votes
                    .Where(v => v.UserId == userId && v.PollId == p.Id)
                    .Select(v => (int?)v.OptionId)
                    .FirstOrDefault()
            }).ToList();

            return result;
        }

        public async Task<(bool success, string? error)> CreatePollAsync(int userId, PollDto pollDto)
        {
            var user = await _context.User.FindAsync(userId);

            if (user.Rank == UserRanksEnum.HomeCook || user.Rank == UserRanksEnum.ChefMaster)
            {
                return (false, "Rank level too low");
            }

            if (pollDto.Options.Count < 2 || string.IsNullOrWhiteSpace(pollDto.Question))
            {
                return (false, "Missing or incomplete field(s)");
            }

            var poll = new Poll
            {
                AuthorId = userId,
                Question = pollDto.Question
            };

            _context.Poll.Add(poll);
            await _context.SaveChangesAsync();

            foreach (var optionDto in pollDto.Options)
            {
                _context.PollOption.Add(new PollOption
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
            var existingVote = await _context.Vote.FirstOrDefaultAsync(v => v.UserId == userId && v.PollId == pollId);

            if (existingVote != null)
            {
                return (false, "User already voted");
            }

            var poll = await _context.Poll.FindAsync(pollId);

            if (poll == null)
            {
                return (false, "Poll not found");
            }

            var pollOptions = await _context.PollOption
                .Where(po => po.PollId == pollId)
                .Select(po => po.Id)
                .ToListAsync();

            if (!pollOptions.Contains(optionId))
            {
                return (false, "Invalid option");
            }

            _context.Vote.Add(new Vote
            {
                UserId = userId,
                PollId = pollId,
                OptionId = optionId
            });

            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool success, string? error, bool forbidden)> DeletePollAsync(int userId, int pollId)
        {
            var poll = await _context.Poll.FindAsync(pollId);

            if (poll == null)
            {
                return (false, "Poll not found", false);
            }

            if (poll.AuthorId != userId)
            {
                return (false, "You are not allowed to delete this poll", true);
            }

            _context.Poll.Remove(poll);
            await _context.SaveChangesAsync();

            return (true, null, false);
        }

        public async Task<(bool success, bool wasUpdated, string? error)> UpdatePollAsync(int userId, int pollId, PollDto updateDto)
        {
            var poll = await _context.Poll.FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll == null)
            {
                return (false, false, "Poll not found");
            }

            if (poll.AuthorId != userId)
            {
                return (false, false, "You are not allowed to update this poll");
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
                var existingOptions = _context.PollOption.Where(o => o.PollId == pollId);

                _context.PollOption.RemoveRange(existingOptions);

                foreach (var optionDto in updateDto.Options)
                {
                    _context.PollOption.Add(new PollOption
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
