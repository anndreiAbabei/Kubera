using Kubera.General.Settings;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kubera.App.Infrastructure.Cors
{
    [DebuggerDisplay("Origins: {string.Join(Origins)}, AllowHeaders: {AllowHeaders}, AllowMethods: {AllowMethods}, AllowedCredentials: {AllowedCredentials}")]
    public class CorsOptions : ICorsOptions
    {
        public IEnumerable<string> Origins { get; set; }

        public string AllowHeaders { get; set; }

        public string AllowMethods { get; set; }

        public bool AllowedCredentials { get; set; }
    }
}
