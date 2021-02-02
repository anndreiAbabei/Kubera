using Kubera.General.Models;

namespace Kubera.Data.Models
{
    internal sealed class PagingResult : IPagingResult
    {
        public uint TotalPages { get; }

        internal PagingResult(int totalPages)
        {
            TotalPages = (uint)totalPages;
        }
    }
}
