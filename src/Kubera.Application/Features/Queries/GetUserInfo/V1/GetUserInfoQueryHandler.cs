using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Kubera.Data.Entities;
using Kubera.Data.Entities.Meta;
using Kubera.Application.Common.Extensions;
using Kubera.General.Services;
using Kubera.Application.Common;

namespace Kubera.Application.Features.Queries.GetUserInfo.V1
{
    public class GetUserInfoQueryHandler : CachingHandler<GetUserInfoQuery, UserInfoModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserInfoQueryHandler(IUserCacheService cacheService, 
            IUserRepository userRepository, 
            IMapper mapper)
            : base(cacheService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        protected override async ValueTask<IResult<UserInfoModel>> HandleImpl(GetUserInfoQuery request, CancellationToken cancellationToken)
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
