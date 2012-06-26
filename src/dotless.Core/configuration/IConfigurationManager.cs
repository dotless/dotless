namespace dotless.Core.configuration
{
    using System.Configuration;

    /// <summary>
    ///  Provides access to config sections.
    /// </summary>
    public interface IConfigurationManager
    {
        T GetSection<T>(string sectionName);
    }

    /// <summary>
    ///  Wraps <seealso cref="ConfigurationManager"/> for <seealso cref="IConfigurationManager"/> implementation.
    /// </summary>
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        public T GetSection<T>(string sectionName)
        {
            return (T) ConfigurationManager.GetSection(sectionName);
        }
    }
}