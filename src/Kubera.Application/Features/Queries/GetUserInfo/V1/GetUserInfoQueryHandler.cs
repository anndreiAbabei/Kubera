using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Extensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Kubera.Data.Entities;
using Kubera.Data.Entities.Meta;
using Kubera.Application.Common.Extensions;

namespace Kubera.Application.Features.Queries.GetUserInfo.V1
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, IResult<UserInfoModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserInfoQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IResult<UserInfoModel>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userRepository.GetMe(cancellationToken)
                                                        .ConfigureAwait(false);

            var result = new UserInfoModel
            {
                Email = user.Email,
                FullName = user.FullName,
                Settings = _mapper.Map<UserSettings, UserSettingsModel>(user.GetSettings())
            };

            return result.AsResult();
        }
    }
}
