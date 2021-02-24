using Kubera.General.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kubera.Data.Entities
{
    public class Currency : Entity, ISoftDeletable
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Symbol { get; set; }

        public virtual int Order { get; set; }

        public virtual bool Deleted { get; set; }

        [NotMapped]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [NotMapped]
        public virtual ICollection<Transaction> FeeTransactions { get; set; }
    }
}
