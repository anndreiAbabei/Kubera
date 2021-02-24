using Kubera.General.Entities;
using System;
using System.Collections.Generic;

namespace Kubera.Data.Entities
{
    public class Group : Entity, ISoftDeletable
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string OwnerId { get; set; } 

        public virtual DateTime CreatedAt { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
