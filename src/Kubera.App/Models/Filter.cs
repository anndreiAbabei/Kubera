using Kubera.General.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Kubera.App.Models
{
    [DebuggerDisplay("{From}-{To} [Asset: {AssetId}, Group: {GroupId}]")]
    public class Filter : IFilter
    {
        [FromQuery(Name = "from")]
        public DateTime? From { get; set; }

        [FromQuery(Name = "to")]
        public DateTime? To { get; set; }

        [FromQuery(Name = "assetId")]
        public Guid? AssetId { get; set; }

        [FromQuery(Name = "groupId")]
        public Guid? GroupId { get; set; }

        public override string ToString() => $"{From?.ToString("o") ?? string.Empty}-" +
                                             $"{To?.ToString("o") ?? string.Empty}-" +
                                             $"{AssetId?.ToString() ?? string.Empty}-" +
                                             $"{AssetId?.ToString() ?? string.Empty}";
    }
}
