namespace Services.ShoppingCart.API.RabbitMQSender;

public interface IRabbitMQCartMessageSender
{
    void SendMessage(Object message, string queueName);
}
