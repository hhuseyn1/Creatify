using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Services.Order.API.RabbitMQSender;

public class RabbitMQOrderMessageSender : IRabbitMQOrderMessageSender
{
    private readonly string _hostName;
    private readonly string _username;
    private readonly string _password;
    private IConnection _connection;
    private const string OrderCreated_RewardUpdateQueue = "RewardsUpdateQueue";
    private const string OrderCreated_EmailUpdateQueue = "EmailUpdateQueue";
    public RabbitMQOrderMessageSender()
    {
        _hostName = "localhost";
        _username = "guest";
        _password = "guest";
    }
    public void SendMessage(object message, string exchangeName)
    {
        if (ConnectionExists())
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: false);
            channel.QueueDeclare(OrderCreated_EmailUpdateQueue,false,false,false,null);
            channel.QueueDeclare(OrderCreated_RewardUpdateQueue, false,false,false,null);

            channel.QueueBind(OrderCreated_EmailUpdateQueue,exchangeName,"EmailUpdate");
            channel.QueueBind(OrderCreated_RewardUpdateQueue, exchangeName, "RewardsUpdate");

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: exchangeName, "EmailUpdate", null, body: body);
            channel.BasicPublish(exchange: exchangeName, "RewardsUpdate", null, body: body);
        }
    }

    private void CreateConnection()
    {
        try
        {

            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Password = _password,
                UserName = _username
            };

            _connection = factory.CreateConnection();
        }
        catch (Exception ex)
        {

        }
    }

    private bool ConnectionExists()
    {
        if (_connection != null)
        {
            return true;
        }
        CreateConnection();
        return true;
    }
}
