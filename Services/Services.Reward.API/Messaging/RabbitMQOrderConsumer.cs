using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Reward.API.Message;
using Services.Reward.API.Services;
using System.Text;

namespace Services.Reward.API.Messaging;

public class RabbitMQOrderConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly RewardService _rewardService;
    private const string OrderCreated_RewardUpdateQueue = "RewardsUpdateQueue";
    private string ExchangeName = "";
    private IConnection _connection;
    private IModel _channel;
    public RabbitMQOrderConsumer(IConfiguration configuration, RewardService rewardService, IConnection connection, IModel channel)
    {
        _configuration = configuration;
        _rewardService = rewardService;
        ExchangeName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
        _channel.QueueDeclare(OrderCreated_RewardUpdateQueue, false, false, false, null);
        _channel.QueueBind(OrderCreated_RewardUpdateQueue, ExchangeName, "RewardsUpdate");
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            RewardsMessage rewardsMessage = JsonConvert.DeserializeObject<RewardsMessage>(content);
            HandleMessage(rewardsMessage).GetAwaiter().GetResult();

            _channel.BasicAck(ea.DeliveryTag, false);
        };
        _channel.BasicConsume(OrderCreated_RewardUpdateQueue, false, consumer);
        return Task.CompletedTask;
    }

    private async Task HandleMessage(RewardsMessage rewardsMessage)
    {
        _rewardService.UpdateRewards(rewardsMessage).GetAwaiter().GetResult();
    }
}
