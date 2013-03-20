using System;

namespace SiliconShark.PoQu.Extensions
{
    using Microsoft.Practices.ServiceLocation;

    internal static class ServiceLocatorExtensions
    {
        internal static T SafeGetInstance<T>(this IServiceLocator locator)
        {
            try
            {
                return locator.GetInstance<T>();
            }
            catch (ActivationException)
            {
                return default(T);
            }
        }

        internal static T GetInstanceWithDefault<T>(this IServiceLocator locator, Func<T> defaultValue)
        {
            try
            {
                return locator.GetInstance<T>();
            }
            catch (ActivationException)
            {
                return defaultValue();
            }
        }
    }
}