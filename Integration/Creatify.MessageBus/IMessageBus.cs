namespace Creatify.MessageBus;

public interface IMessageBus
{
    Task PublishMessage(object message, string topic_queue_name);
}
