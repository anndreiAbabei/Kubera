using System.Collections.Generic;

namespace Kubera.General.Settings
{
    public interface IAutorisationSettings
    {
        string Authority { get; }

        string Audience { get; }

        IEnumerable<string> ValidIssuers{ get; }

        bool MapInboundClaims { get; }
    }
}
