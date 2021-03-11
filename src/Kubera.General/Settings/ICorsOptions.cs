using System.Collections.Generic;

namespace Kubera.General.Settings
{
    public interface ICorsOptions
    {
        IEnumerable<string> Origins { get; }
        string AllowHeaders { get; }
        string AllowMethods { get; }
        bool AllowedCredentials { get; }
    }
}
