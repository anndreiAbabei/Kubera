using System;
using System.Collections.Generic;

namespace Kubera.Data.Entities.Meta
{
    public class UserSettings
    {
        public string Language { get; set; }

        public Guid PrefferedCurrency { get; set; }

        public IEnumerable<Guid> Currencies { get; set; }
    }
}
