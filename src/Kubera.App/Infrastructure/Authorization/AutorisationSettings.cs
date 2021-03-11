using Kubera.General.Settings;
using System.Collections.Generic;

namespace Kubera.App.Infrastructure.Authorization
{
    public class AutorisationSettings : IAutorisationSettings
    {
        public string Authority { get; set; }

        public string Audience { get; set; }

        public IEnumerable<string> ValidIssuers { get; set; }

        public bool MapInboundClaims { get; set; }
    }
}
