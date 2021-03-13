using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Code: {Code}, Name: {Name}")]
    public class GroupInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(16)]
        public virtual string Code { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(128)]
        public virtual string Name { get; set; }
    }

    [DebuggerDisplay("Code: {Code}, Name: {Name}")]
    public class GroupUpdateModel : GroupInputModel { }
}
