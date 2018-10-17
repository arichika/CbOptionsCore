using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace CbOptionsCore
{
    internal class CbOptionsAttributeBindingProvider : IBindingProvider
    {

        private readonly IServiceProvider _serviceProvider;

        public CbOptionsAttributeBindingProvider(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider;

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var parameter = context.Parameter;
            var  attribute = parameter.GetCustomAttribute<CbOptionsAttribute>(inherit: false);

            return Task.FromResult<IBinding>(attribute == null ? null : new CbOptionsBinding(parameter));
        }

        private class CbOptionsBinding : IBinding
        {
            private readonly ParameterInfo _parameter;

            public CbOptionsBinding(ParameterInfo parameter)
            {
                _parameter = parameter;
            }

            public bool FromAttribute => true;

            public Task<IValueProvider> BindAsync(BindingContext context)
            {
                return Task.FromResult<IValueProvider>(new CbOptionsValueBinder(_parameter));
            }

            public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
            {
                throw new NotImplementedException();
            }

            public ParameterDescriptor ToParameterDescriptor()
            {
                return new ParameterDescriptor
                {
                    Name = _parameter.Name,
                };
            }

            private class CbOptionsValueBinder : IValueProvider
            {
                private readonly object _parameter;

                public CbOptionsValueBinder(object parameter) => _parameter = parameter;

                public Type Type => _parameter.GetType();

                public  Task<object> GetValueAsync()
                {
                    var (sectionKey, optionalSettingJsonPath, optional, reloadOnChange) = GetAttributes();

                    return Task.FromResult(CbOptions.Create(
                        Type,
                        new CbOptionsAttribute(
                            sectionKey,
                            optionalSettingJsonPath,
                            optional,
                            reloadOnChange)));
                }

                public string ToInvokeString()
                {
                    var v = GetAttributes();

                    return $"sectionKey={v.sectionKey}, optionalSettingJsonPath={v.optionalSettingJsonPath}, optional={v.optional}, reloadOnChange={v.reloadOnChange}";
                }

                private (string sectionKey, string optionalSettingJsonPath, bool optional, bool reloadOnChange) GetAttributes()
                {

                    var attribute = (CbOptionsAttribute)_parameter;

                    // var attribute = _parameter.GetCustomAttribute<CbOptionsAttribute>();

                    var key = attribute?.SectionKey ?? string.Empty;
                    var path = attribute?.SettingJsonPath ?? string.Empty;
                    var optional = attribute?.Optional ?? true;
                    var reload = attribute?.ReloadOnChange ?? true;

                    return (key, path, optional, reload);
                }
            }
        }
    }
}
