using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace Kubera.Data.Entities.Meta
{
    [DebuggerDisplay("Language: {Language}, PrefferedCurrency: {PrefferedCurrency}")]
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
