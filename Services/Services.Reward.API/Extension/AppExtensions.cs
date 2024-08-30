using Services.Reward.API.Messaging;

namespace Services.Reward.API.Extension;

public static class AppExtensions
{
    private static IAzureServiceBusConsumer AzureServiceBusConsumer { get; set; }
    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
    {
        AzureServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
        var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

        hostApplicationLife.ApplicationStarted.Register(OnStart);
        hostApplicationLife.ApplicationStopping.Register(OnStop);

        return app;
    }

    private static void OnStop()
    {
        AzureServiceBusConsumer.Stop();
    }

    private static void OnStart()
    {
        AzureServiceBusConsumer.Start();
    }
}
