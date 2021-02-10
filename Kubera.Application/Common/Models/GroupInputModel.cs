using System.ComponentModel.DataAnnotations;

namespace Kubera.Application.Common.Models
{
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

    public class GroupUpdateModel : GroupInputModel { }
}
