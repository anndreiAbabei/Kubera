using System;

namespace Kubera.Application.Common.Models
{
    public class CurrencyModel
    {
        public virtual Guid Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Symbol { get; set; }

        public virtual int Order { get; set; }
    }
}
