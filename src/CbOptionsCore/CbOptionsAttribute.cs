using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Description;

namespace CbOptionsCore
{
    /// <summary>
    /// Specifies the usage of CbOptions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
    [Binding]
    public sealed class CbOptionsAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the CbOptionsAttribute class with the specified parameters.
        /// </summary>
        /// <param name="sectionKey"></param>
        /// <param name="settingJsonPath"></param>
        /// <param name="optional"></param>
        /// <param name="reloadOnChange"></param>
        public CbOptionsAttribute(string sectionKey, string settingJsonPath = "local.settings.json", bool optional = true, bool reloadOnChange = false)
        {
            SectionKey = sectionKey;
            SettingJsonPath = settingJsonPath;
            Optional = optional;
            ReloadOnChange = reloadOnChange;
        }

        /// <summary>
        /// The key of the configuration section.
        /// </summary>
        //[AutoResolve]
        public string SectionKey { get; }

        /// <summary>
        /// Path relative to the base path stored in.
        /// </summary>
        //[AutoResolve]
        public string SettingJsonPath { get; }

        /// <summary>
        /// Whether the file is optional.
        /// </summary>
        //[AutoResolve]
        public bool Optional { get; }

        /// <summary>
        /// Whether the configuration should be reloaded if the file changes.
        /// </summary>
        //[AutoResolve]
        public bool ReloadOnChange { get; }


        public override bool Equals(object obj)
        {
            return obj is CbOptionsAttribute attribute &&
                   base.Equals(obj) &&
                   SectionKey == attribute.SectionKey &&
                   SettingJsonPath == attribute.SettingJsonPath &&
                   Optional == attribute.Optional &&
                   ReloadOnChange == attribute.ReloadOnChange;
        }

        public override int GetHashCode()
        {
            var hashCode = 8775747;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SectionKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SettingJsonPath);
            hashCode = hashCode * -1521134295 + Optional.GetHashCode();
            hashCode = hashCode * -1521134295 + ReloadOnChange.GetHashCode();
            return hashCode;
        }
    }
}
