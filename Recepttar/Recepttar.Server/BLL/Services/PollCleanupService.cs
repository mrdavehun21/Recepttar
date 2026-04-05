using Recepttar.Server.BLL.Interfaces;

namespace Recepttar.Server.BLL.Services
{
    public class PollCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PollCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var delay = GetNextMonday() - DateTime.UtcNow;

                if (delay < TimeSpan.Zero)
                {
                    delay = TimeSpan.FromDays(7);
                }
                await Task.Delay(delay, ct);

                using var scope = _scopeFactory.CreateScope();
                var pollService = scope.ServiceProvider.GetRequiredService<IPollService>();

                await pollService.DeactivateAllPollsAsync(ct);
            }
        }

        private static DateTime GetNextMonday()
        {
            var now = DateTime.UtcNow;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;

            if (daysUntilMonday == 0)
            {
                daysUntilMonday = 7;
            }

            return now.Date.AddDays(daysUntilMonday);
        }
    }
}