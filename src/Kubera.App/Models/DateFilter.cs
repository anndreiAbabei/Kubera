using Kubera.General.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Kubera.App.Models
{
    [DebuggerDisplay("{From}-{To}")]
    public class DateFilter : IDateFilter
    {
        [FromQuery(Name = "from")]
        public DateTime? From { get; set; }

        [FromQuery(Name = "to")]
        public DateTime? To { get; set; }

        public override string ToString() => $"{From?.ToString("o") ?? string.Empty}-{To?.ToString("o") ?? string.Empty}";
    }
}
