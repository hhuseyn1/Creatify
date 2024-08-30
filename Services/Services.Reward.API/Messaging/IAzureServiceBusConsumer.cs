namespace Services.Reward.API.Messaging;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}
