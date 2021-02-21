using Kubera.General.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Kubera.App.Models
{
    [DebuggerDisplay("Page: {Page} of {Items} items")]
    public class Paging : IPaging
    {
        [FromQuery(Name = "page")]
        public uint Page { get; set; }

        [FromQuery(Name = "items")]
        public uint Items { get; set; } = 50;

        [NotMapped]
        public IPagingResult Result { get; set; }

        public static IPaging All { get; } = new Paging
        {
            Items = int.MaxValue,
            Page = 0
        };

        public override string ToString() => $"{Page} of {Items}";
    }
}
