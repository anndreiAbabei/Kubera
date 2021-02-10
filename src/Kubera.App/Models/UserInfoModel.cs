using Kubera.Business.Entities;

namespace Kubera.App.Models
{
    public class UserInfoModel
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public UserSettings Settings { get; set; }
    }
}
