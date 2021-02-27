using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Kubera.Data.Entities.Meta
{
    public class UserSettings
    {
        public string Language { get; set; }

        public Guid PrefferedCurrency { get; set; }

        public IEnumerable<Guid> Currencies { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
