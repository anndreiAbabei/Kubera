using Kubera.General.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kubera.Data.Entities
{
    public class Asset : Entity, ISoftDeletable
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Symbol { get; set; }

        public virtual int Order { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual Guid GroupId { get; set; }

        public virtual string OwnerId { get; set; }

        public virtual string Icon { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual Group Group { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
