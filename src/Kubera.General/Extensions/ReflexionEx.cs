using System;
using System.Linq;

namespace Kubera.General.Extensions
{
    public static class ReflexionEx
    {
        public static bool Implements<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }

        public static string AsQueryString(this object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var strings = source.GetType()
                .GetProperties()
                .Select(p => (p.Name, Value: p.GetValue(source)))
                .Where(t => t.Value != null)
                .Select(t => $"{t.Name}={t.Value.ToString().EncodeQueryString()}");

            return string.Join("&", strings);
        }
    }
}
