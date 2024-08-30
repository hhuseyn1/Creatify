namespace Services.Email.API.Messaging;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}
