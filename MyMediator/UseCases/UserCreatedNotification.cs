using System.Threading.Channels;
using MyMediator.Core;

namespace MyMediator.UseCases;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(Guid notificationUserId, string notificationName);
}

public class EmailService : IEmailService
{
    public Task SendWelcomeEmailAsync(Guid notificationUserId, string notificationName)
    {
        return Task.CompletedTask;
    }
}
public class UserCreatedNotification : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
}

public class EmailNotificationHandler : BackgroundService
{
    private readonly IEmailService _emailService;
    
    private readonly InMemoryMessageQueue _channel;

    public EmailNotificationHandler(InMemoryMessageQueue queue)
    {
        _channel = queue;
    }
    
    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        await _emailService.SendWelcomeEmailAsync(notification.UserId, notification.Name);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
              
            }
            catch (Exception e)
            {
                    
            }
        }
    }
}
