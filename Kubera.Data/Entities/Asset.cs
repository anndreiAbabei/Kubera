using System;
using System.Collections.Generic;

namespace Kubera.Data.Entities
{
    public class Asset : Currency
    {
        public virtual DateTime CreatedAt { get; set; }

        public virtual Guid GroupId { get; set; }

        public virtual string OwnerId { get; set; }

        public virtual string Icon { get; set; }

        public virtual Group Group { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
