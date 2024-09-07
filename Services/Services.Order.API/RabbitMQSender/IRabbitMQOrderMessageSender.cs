namespace Services.Order.API.RabbitMQSender;

public interface IRabbitMQOrderMessageSender
{
    void SendMessage(Object message, string exchangeName);
}
