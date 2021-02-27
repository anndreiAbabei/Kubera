using System.Text.Json;
using Kubera.Data.Entities;
using Kubera.Data.Entities.Meta;

namespace Kubera.Data.Extensions
{
    public static class EntityEx
    {
        public static UserSettings GetSettings(this ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Settings))
                return null;

            return JsonSerializer.Deserialize<UserSettings>(user.Settings);
        }
    }
}
