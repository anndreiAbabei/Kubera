namespace Kubera.App.Models
{
    public class CurrencyModel
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Symbol { get; set; }

        public virtual int Order { get; set; }
    }
}
