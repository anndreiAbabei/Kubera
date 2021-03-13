using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Email: {Email}, FullName: {FullName}")]
    public class UserInfoModel
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public UserSettingsModel Settings { get; set; }
    }

    [DebuggerDisplay("Language: {Language}, PrefferedCurrency: {PrefferedCurrency}")]
    public class UserSettingsModel
    {
        public string Language { get; set; }

        public Guid PrefferedCurrency { get; set; }

        public IEnumerable<Guid> Currencies { get; set; }
    }
}
