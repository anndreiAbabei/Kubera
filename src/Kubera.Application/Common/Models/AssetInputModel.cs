using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Code: {Code}, Name: {Name}, Symbol: {Symbol}, Order: {Order}")]
    public class AssetInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(16)]
        public virtual string Code { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(128)]
        public virtual string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(16)]
        public virtual string Symbol { get; set; }

        public virtual int Order { get; set; }

        [Required]
        public virtual Guid GroupId { get; set; }

        public virtual string Icon { get; set; }
    }

    [DebuggerDisplay("Code: {Code}, Name: {Name}, Symbol: {Symbol}, Order: {Order}")]
    public class AssetUpdateModel : AssetInputModel { }
}
