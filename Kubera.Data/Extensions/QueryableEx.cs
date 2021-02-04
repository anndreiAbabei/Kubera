using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Data.Extensions
{
    public static class QueryableEx
    {
        public static async ValueTask<IList<T>> ToListAsync<T>(this ValueTask<IQueryable<T>> source, CancellationToken cancellationToken = default)
        {
            return await (await source.ConfigureAwait(false))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
