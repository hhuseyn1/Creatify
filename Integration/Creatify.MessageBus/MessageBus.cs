using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using Creatify.MessageBus;

public class MessageBus : IMessageBus
{
    private readonly string connectionString;

    public MessageBus()
    {
        var keyVaultUrl = "https://azservicebuskey.vault.azure.net/";

        var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

        try
        {
            KeyVaultSecret secret = secretClient.GetSecret("AzureServiceBusConnection");
            Console.WriteLine(secret);
            connectionString = secret.Value;
                
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Azure Service Bus connection string is missing in Key Vault.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving secret from Key Vault: {ex.Message}");
        }
    }

    public async Task PublishMessage(object message, string topic_queue_name)
    {
        await using var client = new ServiceBusClient(connectionString);
        ServiceBusSender busSender = client.CreateSender(topic_queue_name);

        var jsonMessage = JsonConvert.SerializeObject(message);
        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };

        await busSender.SendMessageAsync(serviceBusMessage);
        await client.DisposeAsync();
    }
}
