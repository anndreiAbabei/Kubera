using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetUserInfo.V1
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, Result<UserInfoModel>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserInfoQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserInfoModel>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetMe(cancellationToken)
                   .ConfigureAwait(false);

            var result = new UserInfoModel
            {
                Email = user.Email,
                FullName = user.FullName,
                Settings = JsonSerializer.Deserialize<UserSettingsModel>(user.Settings)
            };

            return result;
        }
    }
}
