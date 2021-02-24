using System;
using System.Collections.Generic;

namespace Kubera.Application.Common.Models
{
    public class UserInfoModel
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public UserSettingsModel Settings { get; set; }
    }

    public class UserSettingsModel
    {
        public string Language { get; set; }

        public Guid PrefferedCurrency { get; set; }

        public IEnumerable<Guid> Currencies { get; set; }
    }
}
