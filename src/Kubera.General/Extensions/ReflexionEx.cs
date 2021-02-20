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
                .Select(p => $"{p.Name}={p.GetValue(source).ToString().EncodeQueryString()}");

            return string.Join("&", strings);
        }
    }
}
