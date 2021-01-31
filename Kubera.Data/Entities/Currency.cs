using Kubera.General.Entities;
using System.Collections.Generic;

namespace Kubera.Data.Entities
{
    public class Currency : Entity, ISoftDeletable
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Symbol { get; set; }

        public virtual int Order { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Transaction> FeeTransactions { get; set; }
    }
}
