using Recepttar.Server.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.DAL.Interfaces;

namespace Recepttar.Server.DAL.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly AppDbContext _context;

        public PollRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Poll>> GetAllAsync()
        {
            return await _context.Polls
                .Include(p => p.Author)
                .Include(p => p.Options)
                    .ThenInclude(o => o.Votes)
                .ToListAsync();
        }

        public async Task<Poll?> GetByIdAsync(int pollId)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == pollId);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> OptionBelongsToPollAsync(int pollId, int optionId)
        {
            return await _context.PollOptions.AnyAsync(po => po.PollId == pollId && po.Id == optionId);
        }

        public async Task<Vote?> GetExistingVoteAsync(int userId, int pollId)
        {
            var pollOptionIds = await _context.PollOptions
                .Where(po => po.PollId == pollId)
                .Select(po => po.Id)
                .ToListAsync();

            return await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && pollOptionIds.Contains(v.OptionId));
        }

        public async Task AddPollAsync(Poll poll)
        {
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();
        }

        public async Task AddVoteAsync(Vote vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePollAsync(Poll poll)
        {
            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceOptionsAsync(int pollId, List<PollOption> options)
        {
            var existing = _context.PollOptions.Where(o => o.PollId == pollId);
            _context.PollOptions.RemoveRange(existing);
            _context.PollOptions.AddRange(options);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePollAsync(Poll poll)
        {
            _context.Polls.Update(poll);
            await _context.SaveChangesAsync();
        }
    }
}
