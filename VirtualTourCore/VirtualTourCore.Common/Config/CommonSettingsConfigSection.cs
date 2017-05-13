using System.Configuration;

namespace VirtualTourCore.Common.Config
{
    public class CommonSettingsConfigSection : ConfigurationSection
    {
        #region Constants

        /// <summary>
        /// Nested static class containing only the constant names of the configuration settings. 
        /// This is so there are no mis-match spelling errors or if the developer wants to reference them
        /// another way and wants to know the actual configuration name without having to look it up.
        /// </summary>
        public static class PropertyNames
        {
            /// <summary>
            /// The dbContextCommandTimeout property's name.
            /// </summary>
            public const string DbContextCommandTimeout = "dbContextCommandTimeout";
        }

        #endregion

        /// <summary>
        /// Returns an instance of the <see cref="ServicesManagement.Common.Config.CommonSettingsConfiguration"/> section from 
        /// the <see cref="System.Configuration.ConfigurationManager"/>'s current config file.
        /// </summary>
        /// <returns></returns>
        public static CommonSettingsConfigSection GetSection()
        {
            return (CommonSettingsConfigSection)ConfigurationManager.GetSection("CommonSettings");
        }

        /// <summary>
        /// Gets or sets the configuration value for the Timeout length in seconds 
        /// of the DbContext's DbCommand, this property is required in the configuration section.
        /// </summary>
        [ConfigurationProperty(PropertyNames.DbContextCommandTimeout, IsRequired = true)]
        public int DbContextCommandTimeout
        {
            get { return (int)this[PropertyNames.DbContextCommandTimeout]; }
            set { this[PropertyNames.DbContextCommandTimeout] = value; }
        }
    }
}
