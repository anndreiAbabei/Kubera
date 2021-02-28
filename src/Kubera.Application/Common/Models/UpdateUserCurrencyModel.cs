using System;
using System.ComponentModel.DataAnnotations;

namespace Kubera.Application.Common.Models
{
    public class UpdateUserCurrencyModel
    {
        [Required]
        public Guid CurrencyId { get; set; }
    }
}
