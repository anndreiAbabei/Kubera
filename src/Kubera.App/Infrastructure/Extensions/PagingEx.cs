using System;
using System.Globalization;
using Kubera.General.Models;
using Microsoft.AspNetCore.Http;

namespace Kubera.App.Infrastructure.Extensions
{
    public static class PagingEx
    {
        public const string HeaderNumberOfPagesName = "X-PageCount";

        public static void AddPaging(this HttpContext httpContext, IPagingResult pagingResult)
        {
            if (httpContext == null)
                return;

            if (pagingResult == null)
                throw new ArgumentNullException(nameof(pagingResult));

            httpContext.Response.Headers.Add(HeaderNumberOfPagesName, pagingResult.TotalItems.ToString(CultureInfo.InvariantCulture));
        }
    }
}
