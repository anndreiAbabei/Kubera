using System.Diagnostics;

namespace Kubera.Application.Features.Models
{
    [DebuggerDisplay("Count: {Count}")]
    public class CollectionOutputModel
    {
        public int Count { get; set; }
    }
}
