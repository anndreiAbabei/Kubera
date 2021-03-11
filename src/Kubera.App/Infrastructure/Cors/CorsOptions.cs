using Kubera.General.Settings;
using System.Collections.Generic;

namespace Kubera.App.Infrastructure.Cors
{
    public class CorsOptions : ICorsOptions
    {
        public IEnumerable<string> Origins { get; set; }

        public string AllowHeaders { get; set; }

        public string AllowMethods { get; set; }

        public bool AllowedCredentials { get; set; }
    }
}
