using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Email.API.Services;
using Services.Email.Models.Dto;
using System.Text;

namespace Services.Email.API.Messaging;

public class RabbitMQCartConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;
    private string queueName;
    private IConnection _connection;
    private IModel _channel;
    public RabbitMQCartConsumer(IConfiguration configuration, EmailService emailService)
    {
        _configuration = configuration;
        _emailService = emailService;
        queueName = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queueName, false, false, false, null);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(content);
            HandleMessage(cartDto).GetAwaiter().GetResult();

            _channel.BasicAck(ea.DeliveryTag, false);
        };
        _channel.BasicConsume(queueName, false, consumer);
        return Task.CompletedTask;
    }

    private async Task HandleMessage(CartDto cartDto)
    {
        _emailService.EmailCartAndLog(cartDto).GetAwaiter().GetResult();
    }
}
