using Kubera.General.Settings;

namespace Kubera.App.Infrastructure.Authorization
{
    public class AutorisationSettings : IAutorisationSettings
    {
        public string Authority { get; set; }

        public string Audience { get; set; }

        public string ValidIssuer { get; set; }

        public bool MapInboundClaims { get; set; }
    }
}
