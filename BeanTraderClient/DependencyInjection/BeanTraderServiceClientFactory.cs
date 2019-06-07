using Microsoft.Extensions.DependencyInjection;
using ServiceModel.Configuration;
using System;
using System.ServiceModel;

namespace BeanTraderClient.DependencyInjection
{
    public sealed class BeanTraderServiceClientFactory : IDisposable
    {
        private readonly ServiceProvider provider;

        public BeanTraderServiceClientFactory(BeanTraderServiceCallback callbackHandler)
        {
            provider = CreateServiceProvider(callbackHandler);
        }

        public BeanTraderServiceClient GetServiceClient()
        {
            return provider.GetRequiredService<BeanTraderServiceClient>();
        }

        private static ServiceProvider CreateServiceProvider(BeanTraderServiceCallback callback)
        {
            var services = new ServiceCollection();

            services.AddSingleton(callback);
            services.AddServiceModelClient()
                .AddConfigurationManagerFile("BeanTraderClient.exe.config");
            services.AddSingleton<BeanTraderServiceClient>();

            return services.BuildServiceProvider();
        }

        public void Dispose() => provider.Dispose();
    }
}

public partial class BeanTraderServiceClient
{
    public BeanTraderServiceClient(BeanTraderServiceCallback callback, IChannelFactoryProvider provider)
        : base(new InstanceContext(callback), provider.GetEndpoint<BeanTraderService>())
    {
    }
}
