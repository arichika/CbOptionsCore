using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;

namespace CbOptionsCore
{
    public class CbOptionsExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Config.RegisterBindingExtensions(
                new CbOptionsAttributeBindingProvider());
        }
    }
}
