using System.Text.Json;
using Kubera.Data.Entities;

namespace Kubera.Data.Extensions
{
    public static class EntityEx
    {
        public static UserSettings GetSettings(this ApplicationUser user)
        {
            if (user.Settings == null)
                return null;

            return JsonSerializer.Deserialize<UserSettings>(user.Settings);
        }
    }
}
