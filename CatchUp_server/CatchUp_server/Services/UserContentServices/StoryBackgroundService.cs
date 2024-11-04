using CatchUp_server.Db;

namespace CatchUp_server.Services.UserContentServices
{
    public class StoryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public StoryBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanUpExpiredStories();
                await Task.Delay(_checkInterval, stoppingToken); // Wait for the interval before checking again
            }
        }

        private async Task CleanUpExpiredStories()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var storyService = scope.ServiceProvider.GetRequiredService<StoryService>();
                var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

                var expiredStories = context.Stories
                    .Where(s => s.ExpiresAt <= DateTime.Now)
                    .ToList();

                foreach (var story in expiredStories)
                {
                    storyService.Delete(story.Id);
                }
            }
        }
    }
}
