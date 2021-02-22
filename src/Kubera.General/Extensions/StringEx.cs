using System.Collections.Generic;
using System.Web;

namespace Kubera.General.Extensions
{
    public static class StringEx
    {
        public static string EncodeQueryString(this string source) => HttpUtility.UrlEncode(source ?? string.Empty);



        public static string Join(this IEnumerable<string> source, string separator = ", ") => string.Join(separator, source);
    }
}
