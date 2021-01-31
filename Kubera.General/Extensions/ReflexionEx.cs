using System;

namespace Kubera.General.Extensions
{
    public static class ReflexionEx
    {
        public static bool Implements<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }
    }
}
