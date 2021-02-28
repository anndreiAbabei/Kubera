using System;

namespace Kubera.General.Models
{
    public interface IFilter
    {
        DateTime? From { get; }

        DateTime? To { get; }

        Guid? AssetId { get; }

        Guid? GroupId { get; }
    }
}
