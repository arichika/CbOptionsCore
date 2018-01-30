using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace CbOptionsCore
{
    /// <summary>
    /// CbOptions accessor.
    /// </summary>
    public static class CbOptions
    {
        /// <summary>
        /// Configuration cache Instance.
        /// </summary>
        private static ConcurrentDictionary<CbOptionsAttribute, IConfiguration> ConfigurationCache { get; }
            = new ConcurrentDictionary<CbOptionsAttribute, IConfiguration>();

        /// <summary>
        /// Create custom option class by ConfigurationBuilder().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cbOptionsAttribute"></param>
        /// <returns></returns>
        public static T Create<T>(CbOptionsAttribute cbOptionsAttribute)
        {
            return (T)Create(typeof(T), cbOptionsAttribute);
        }

        /// <summary>
        /// Create custom option class by ConfigurationBuilder().
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cbOptionsAttribute"></param>
        /// <returns></returns>
        public static object Create(Type type, CbOptionsAttribute cbOptionsAttribute)
        {
            return ConfigurationCache.GetOrAdd(
                    cbOptionsAttribute,
                    attribute => new ConfigurationBuilder()
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile(
                            path: cbOptionsAttribute.SettingJsonPath,
                            optional: cbOptionsAttribute.Optional,
                            reloadOnChange: cbOptionsAttribute.ReloadOnChange)
                        .AddEnvironmentVariables()
                        .Build())
                .GetSection(cbOptionsAttribute.SectionKey).Get(type);
        }
    }
}
