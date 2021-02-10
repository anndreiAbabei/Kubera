using Kubera.General.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Kubera.Data.Entities
{
    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        public virtual string FullName { get; set; }

        public virtual string Settings { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Asset>  Assets { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
