using Kubera.General.Models;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("TotalItems: {TotalItems}")]
    internal sealed class PagingResult : IPagingResult
    {
        public uint TotalItems { get; }

        internal PagingResult(int totalItems)
        {
            TotalItems = (uint)totalItems;
        }
    }
}
