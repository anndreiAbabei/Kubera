using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("CurrencyId: {CurrencyId}")]
    public class UpdateUserCurrencyModel
    {
        [Required]
        public Guid CurrencyId { get; set; }
    }
}
