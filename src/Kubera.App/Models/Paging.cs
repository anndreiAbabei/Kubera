using Kubera.General.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kubera.App.Models
{
    public class Paging : IPaging
    {
        [FromQuery(Name = "page")]
        public uint Page { get; set; } = 0;

        [FromQuery(Name = "items")]
        public uint Items { get; set; } = 50;

        [NotMapped]
        public IPagingResult Result { get; set; }

        public static IPaging All { get; } = new Paging
        {
            Items = int.MaxValue,
            Page = 0
        };
    }
}
