using Kubera.General.Models;

namespace Kubera.Application.Common.Models
{
    internal sealed class PagingResult : IPagingResult
    {
        public uint TotalItems { get; }

        internal PagingResult(int totalItems)
        {
            TotalItems = (uint)totalItems;
        }
    }
}
