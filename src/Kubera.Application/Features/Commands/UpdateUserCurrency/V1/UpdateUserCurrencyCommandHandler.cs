using CSharpFunctionalExtensions;
using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Services;
using Kubera.Data.Entities.Meta;
using Kubera.Data.Extensions;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.UpdateUserCurrency.V1
{
    public class UpdateUserCurrencyCommandHandler : IRequestHandler<UpdateUserCurrencyCommand, IResult>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public UpdateUserCurrencyCommandHandler(IUserCacheService userCacheService,
            IUserRepository userRepository,
            ICurrencyRepository currencyRepository)
        {
            _userCacheService = userCacheService;
            _userRepository = userRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<IResult> Handle(UpdateUserCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await _currencyRepository.GetById(request.Input.CurrencyId, cancellationToken)
                .ConfigureAwait(false);

            if (currency == null)
                return Result.Failure(ErrorCodes.NotFound);

            var user = await _userRepository.GetMe(cancellationToken)
                .ConfigureAwait(false);


            if (user == null)
                return Result.Failure(ErrorCodes.NotFound);

            var settings = user.GetSettings() ?? new UserSettings();

            settings.PrefferedCurrency = request.Input.CurrencyId;

            user.Settings = settings.ToString();

            await _userRepository.Update(user, cancellationToken)
                .ConfigureAwait(false);

            _userCacheService.Remove(CacheRegion.Users);

            return Result.Success();
        }
    }
}
