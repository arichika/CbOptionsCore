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

            private class CbOptionsValueBinder : ValueBinder
            {
                private readonly ParameterInfo _parameter;

                public CbOptionsValueBinder(ParameterInfo parameter)
                    : base(parameter.ParameterType)
                {
                    _parameter = parameter;
                }

#pragma warning disable 1998
                public override async Task<object> GetValueAsync()
#pragma warning restore 1998
                {
                    var v = GetAttributes();

                    return CbOptions.Create(
                        _parameter.ParameterType
                        , new CbOptionsAttribute(
                            v.sectionKey,
                            v.optionalSettingJsonPath,
                            v.optional,
                            v.reloadOnChange));
                }

                public override string ToInvokeString()
                {
                    var v = GetAttributes();

                    return $"sectionKey={v.sectionKey}, optionalSettingJsonPath={v.optionalSettingJsonPath}, optional={v.optional}, reloadOnChange={v.reloadOnChange}";
                }

                private (string sectionKey, string optionalSettingJsonPath, bool optional, bool reloadOnChange) GetAttributes()
                {
                    var attribute = _parameter.GetCustomAttribute<CbOptionsAttribute>();

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
