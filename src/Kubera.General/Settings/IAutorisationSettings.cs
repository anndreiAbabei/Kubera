using System.Collections.Generic;

namespace Kubera.General.Settings
{
    public interface IAutorisationSettings
    {
        string Authority { get; }

        string Audience { get; }

        string ValidIssuer { get; }

        bool MapInboundClaims { get; }
    }
}
