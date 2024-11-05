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
                DeleteExpiredStories();
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task DeleteExpiredStories()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
                var storyService = scope.ServiceProvider.GetRequiredService<StoryService>();

                var stories = context.Stories
                    .Where(s => s.ExpiresAt <= DateTime.Now)
                    .ToList();

                foreach (var story in stories)
                {
                    storyService.Delete(story.Id);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
