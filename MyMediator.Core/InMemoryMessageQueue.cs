using System.Threading.Channels;

namespace MyMediator.Core;

public class InMemoryMessageQueue
{
    private readonly Channel<IntegrationEvent> _channel =
        Channel.CreateUnbounded<IntegrationEvent>();
    public ChannelReader<IntegrationEvent> Reader => _channel.Reader;
    public ChannelWriter<IntegrationEvent> Writer => _channel.Writer;
}