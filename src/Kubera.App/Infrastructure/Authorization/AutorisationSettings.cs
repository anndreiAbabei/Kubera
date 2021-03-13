using Kubera.General.Settings;
using System.Diagnostics;

namespace Kubera.App.Infrastructure.Authorization
{
    [DebuggerDisplay("Authority: {Authority}, Audience: {Audience}, ValidIssuer: {ValidIssuer}, MapInboundClaims: {MapInboundClaims}")]
    public class AutorisationSettings : IAutorisationSettings
    {
        public string Authority { get; set; }

        public string Audience { get; set; }

        public string ValidIssuer { get; set; }

        public bool MapInboundClaims { get; set; }
    }
}
