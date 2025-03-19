using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Services.Email.API.Message;
using Services.Email.API.Services;
using Services.Email.Models.Dto;
using System.Text;

namespace Services.Email.API.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly string serviceBusConnectionString;
    private readonly string emailCartQueue;
    private readonly string registerUserQueue;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;
    private readonly string orderCreated_Topic;
    private readonly string orderCreated_Email_Subscription;
    private ServiceBusProcessor _emailOrderPlacedProcessor;
    private ServiceBusProcessor _emailCartProcessor;
    private ServiceBusProcessor _registerUserProcessor;

    public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
    {
        this._emailService = emailService;
        this._configuration = configuration;
        serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
        emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
        registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");
        orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
        orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");

        var client = new ServiceBusClient(serviceBusConnectionString);
        _emailCartProcessor = client.CreateProcessor(emailCartQueue);
        _registerUserProcessor = client.CreateProcessor(registerUserQueue);
        _emailOrderPlacedProcessor = client.CreateProcessor(orderCreated_Topic, orderCreated_Email_Subscription);
    }

    public async Task Start()
    {
        _emailCartProcessor.ProcessMessageAsync += OnEmailCartReceived;
        _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
        await _emailCartProcessor.StartProcessingAsync();

        _registerUserProcessor.ProcessMessageAsync += OnRegisterUserReceived;
        _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
        await _registerUserProcessor.StartProcessingAsync();

        _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
        _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
        await _emailOrderPlacedProcessor.StartProcessingAsync();
    }


    public async Task Stop()
    {
        await _emailCartProcessor.StopProcessingAsync();
        await _emailCartProcessor.DisposeAsync();

        await _registerUserProcessor.StopProcessingAsync();
        await _registerUserProcessor.DisposeAsync();

        await _emailOrderPlacedProcessor.StopProcessingAsync();
        await _emailOrderPlacedProcessor.DisposeAsync();
    }

    private async Task OnEmailCartReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(body);

        try
        {
            await _emailService.EmailCartAndLog(cartDto);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task OnRegisterUserReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        string email = JsonConvert.DeserializeObject<string>(body);

        try
        {
            await _emailService.RegisterUserEmailAndLog(email);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        RewardsMessage rewards = JsonConvert.DeserializeObject<RewardsMessage>(body);

        try
        {
            await _emailService.LogOrderPlaced(rewards);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

}
