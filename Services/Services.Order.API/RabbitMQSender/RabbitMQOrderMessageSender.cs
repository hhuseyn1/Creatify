using Newtonsoft.Json;
using RabbitMQ.Client;
using Services.Order.API.RabbitMQSender;
using System.Text;

public class RabbitMQOrderMessageSender : IRabbitMQOrderMessageSender, IDisposable
{
    private readonly string _hostName;
    private readonly string _username;
    private readonly string _password;
    private IConnection _connection;
    private readonly object _connectionLock = new object();
    private const string OrderCreated_RewardUpdateQueue = "RewardsUpdateQueue";
    private const string OrderCreated_EmailUpdateQueue = "EmailUpdateQueue";

    public RabbitMQOrderMessageSender(IConfiguration configuration)
    {
        _hostName = configuration["RabbitMQ:HostName"];
        _username = configuration["RabbitMQ:UserName"];
        _password = configuration["RabbitMQ:Password"];
    }

    public void SendMessage(object message, string exchangeName)
    {
        if (ConnectionExists())
        {
            using var channel = _connection.CreateModel();
            InitializeQueues(channel, exchangeName);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: exchangeName, routingKey: "EmailUpdate", basicProperties: null, body: body);
            channel.BasicPublish(exchange: exchangeName, routingKey: "RewardsUpdate", basicProperties: null, body: body);
        }
    }

    private void InitializeQueues(IModel channel, string exchangeName)
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: false);
        channel.QueueDeclare(OrderCreated_EmailUpdateQueue, false, false, false, null);
        channel.QueueDeclare(OrderCreated_RewardUpdateQueue, false, false, false, null);

        channel.QueueBind(OrderCreated_EmailUpdateQueue, exchangeName, "EmailUpdate");
        channel.QueueBind(OrderCreated_RewardUpdateQueue, exchangeName, "RewardsUpdate");
    }

    private void CreateConnection()
    {
        lock (_connectionLock)
        {
            if (_connection != null && _connection.IsOpen) return;

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
                Console.WriteLine($"RabbitMQ connection error: {ex.Message}");
            }
        }
    }

    private bool ConnectionExists()
    {
        if (_connection != null && _connection.IsOpen)
        {
            return true;
        }
        CreateConnection();
        return _connection != null && _connection.IsOpen;
    }

    public void Dispose()
    {
        if (_connection != null)
        {
            _connection.Dispose();
        }
    }
}
