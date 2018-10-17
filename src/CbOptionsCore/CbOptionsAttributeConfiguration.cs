using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;

namespace CbOptionsCore
{
    public class CbOptionsAttributeConfiguration : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var services = new ServiceCollection();
            //RegisterServices(services);
            var serviceProvider = services.BuildServiceProvider(true);

            context
                .AddBindingRule<CbOptionsAttribute>()
                .Bind(new CbOptionsAttributeBindingProvider(serviceProvider));
        }
        private void RegisterServices(IServiceCollection services)
        {
            throw new NotImplementedException();
            //services.AddSingleton<IT, T>();
        }
    }
}
