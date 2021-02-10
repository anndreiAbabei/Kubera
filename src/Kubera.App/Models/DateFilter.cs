using Kubera.General.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Kubera.App.Models
{
    public class DateFilter : IDateFilter
    {
        [FromQuery(Name = "from")]
        public DateTime? From { get; set; }

        [FromQuery(Name = "to")]
        public DateTime? To { get; set; }
    }
}
