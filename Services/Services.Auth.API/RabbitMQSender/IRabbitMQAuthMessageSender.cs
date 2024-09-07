namespace Services.Auth.API.RabbitMQSender;

public interface IRabbitMQAuthMessageSender
{
    void SendMessage(Object message, string queueName);
}
