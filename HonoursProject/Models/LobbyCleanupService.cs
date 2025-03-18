using HonoursProject.Data;
using Microsoft.Extensions.Hosting;

public class LobbyCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public LobbyCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var cutoff = DateTime.UtcNow.AddHours(-1);

                var expiredLobbies = context.Lobbies
                    .Where(l => l.CreatedDate < cutoff && l.IsActive)
                    .ToList();

                foreach (var lobby in expiredLobbies)
                {
                    lobby.IsActive = false;
                }

                await context.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
