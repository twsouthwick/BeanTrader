using Microsoft.Extensions.DependencyInjection;
using ServiceModel.Configuration;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

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
                .AddConfigurationManagerFile("wcf.config");
            services.AddSingleton<BeanTraderServiceClient>();

            return services.BuildServiceProvider();
        }

        public void Dispose() => provider.Dispose();
    }
}

public partial class BeanTraderServiceClient
{
    public BeanTraderServiceClient(BeanTraderServiceCallback callback, IChannelFactoryProvider provider)
        : this(new InstanceContext(callback), provider.GetEndpoint<BeanTraderService>())
    {
    }

    private BeanTraderServiceClient(InstanceContext callback, ServiceEndpoint endpoint)
        : this(callback, endpoint.Binding, endpoint.Address)
    {

    }
}
